namespace Inventory.Application.InventorySerials.Commands;

using System.Net;

/// <summary>Registers a serial-controlled unit at receipt time (brief §17/§37
/// "AssignSerialNumberCommand") — the returned id is what a caller passes as SerialId when adding an
/// InventoryIssue line.</summary>
public sealed record AssignSerialNumberCommand(long ProductId, string SerialNumber, long StockId, long? LocationId, long? BatchId) : ICommand<long>;

public sealed class AssignSerialNumberCommandHandler(
    IRepository<InventorySerial> serialRepository, IUnitOfWork unitOfWork) : ICommandHandler<AssignSerialNumberCommand, long>
{
    public async Task<Result<long>> Handle(AssignSerialNumberCommand request, CancellationToken cancellationToken)
    {
        if (await serialRepository.AnyAsync(s => s.ProductId == request.ProductId && s.SerialNumber == request.SerialNumber, cancellationToken))
            return new Result<long>(HttpStatusCode.BadRequest, 0, [new Error($"Serial number {request.SerialNumber} already exists for this item.")]);

        try
        {
            var serial = InventorySerial.Receive(request.ProductId, request.SerialNumber, request.StockId, request.LocationId, request.BatchId, DateTime.Now);
            await serialRepository.CreateAsync(serial);
            await unitOfWork.SaveChangeAsync(cancellationToken);
            return new Result<long>(HttpStatusCode.OK, serial.Id, null);
        }
        catch (InventoryDomainException ex)
        {
            return new Result<long>(HttpStatusCode.BadRequest, 0, [new Error(ex.Message)]);
        }
    }
}
