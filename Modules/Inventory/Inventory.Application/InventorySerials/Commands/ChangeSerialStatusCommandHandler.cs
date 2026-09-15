namespace Inventory.Application.InventorySerials.Commands;

using System.Net;

public sealed record MarkSerialReturnedCommand(long SerialId, long StockId, long? LocationId) : ICommand;
public sealed record MarkSerialDamagedCommand(long SerialId) : ICommand;
public sealed record MarkSerialLostCommand(long SerialId) : ICommand;

public sealed class MarkSerialReturnedCommandHandler(
    IRepository<InventorySerial> serialRepository, IUnitOfWork unitOfWork) : ICommandHandler<MarkSerialReturnedCommand>
{
    public async Task<Result> Handle(MarkSerialReturnedCommand request, CancellationToken cancellationToken)
    {
        var serial = await serialRepository.GetByFilterAsync(s => s.Id == request.SerialId, "");
        if (serial is null)
            return new Result(HttpStatusCode.NotFound, [new Error("Serial not found.")]);

        try
        {
            serial.MarkReturned(request.StockId, request.LocationId);
            await serialRepository.UpdateAsync(serial);
            await unitOfWork.SaveChangeAsync(cancellationToken);
            return new Result(HttpStatusCode.OK, null);
        }
        catch (InventoryDomainException ex)
        {
            return new Result(HttpStatusCode.BadRequest, [new Error(ex.Message)]);
        }
    }
}

public sealed class MarkSerialDamagedCommandHandler(
    IRepository<InventorySerial> serialRepository, IUnitOfWork unitOfWork) : ICommandHandler<MarkSerialDamagedCommand>
{
    public async Task<Result> Handle(MarkSerialDamagedCommand request, CancellationToken cancellationToken)
    {
        var serial = await serialRepository.GetByFilterAsync(s => s.Id == request.SerialId, "");
        if (serial is null)
            return new Result(HttpStatusCode.NotFound, [new Error("Serial not found.")]);

        serial.MarkDamaged();
        await serialRepository.UpdateAsync(serial);
        await unitOfWork.SaveChangeAsync(cancellationToken);
        return new Result(HttpStatusCode.OK, null);
    }
}

public sealed class MarkSerialLostCommandHandler(
    IRepository<InventorySerial> serialRepository, IUnitOfWork unitOfWork) : ICommandHandler<MarkSerialLostCommand>
{
    public async Task<Result> Handle(MarkSerialLostCommand request, CancellationToken cancellationToken)
    {
        var serial = await serialRepository.GetByFilterAsync(s => s.Id == request.SerialId, "");
        if (serial is null)
            return new Result(HttpStatusCode.NotFound, [new Error("Serial not found.")]);

        serial.MarkLost();
        await serialRepository.UpdateAsync(serial);
        await unitOfWork.SaveChangeAsync(cancellationToken);
        return new Result(HttpStatusCode.OK, null);
    }
}
