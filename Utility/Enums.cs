namespace Utility
{
    public enum Status { All = 0 , New = 1  , Deleted  = 5 , Approved = 10 , Posted = 15 , Closed = 20 }
    public enum ResultStatus { nothing = 0, success = 1, error = 2 };
    public enum DealerType { Client = 1, Supplier = 2};
    public enum LogType { Index = 0 , View = 1 , Add = 2 , Update = 3 , Delete = 4};
    public enum LogStatus { Success = 0, Wrong = 1, Error = 2 };
    public enum LogAccessLevel { EveryOne = 0 , Admin = 1 , System = 2 , Technical = 3 };
    public enum IndexMode { Elements = 0, Grids = 1 , Bars = 2};
    public enum ShowInIndexMode { All = 0 , Elements = 1, Grids = 2, Bars = 3 };
}