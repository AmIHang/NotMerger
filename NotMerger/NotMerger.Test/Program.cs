using NotMerger.Core.Model.Config;
using NotMerger.Core.Services;
using NotMerger.Core.Services.Merge;
using NotMerger.Core.Services.Parser;
using System.Xml.Serialization;


var o = @"C:\Users\Amy\Downloads\WECORailOperations_TCQC_0.9.0.0\WECOOperations\RailOperations.1.7.0.99\module.config.xaml";
var n = @"C:\Users\Amy\Downloads\WECORailOperations_TCQC_0.9.0.0\WECOOperations\RailOperations.1.7.0.100\module.config.xaml";
var c = @"C:\Users\Amy\Downloads\WECORailOperations_TCQC_0.9.0.0\WECOOperations\module.config.xaml";

var mo = new Module()
{
    Name = "Weco",
    Path = o
};

var mn = new Module()
{
    Name = "Weco",
    Path = n
};

var mc = new Module()
{
    Name = "Weco",
    Path = c
};
var confog = new MergeConfig
{
    CurrentVersion = new ModuleGroup
    {
        Items = [mo]
    },
    NextVersion = new ModuleGroup
    {
        Items = [mn]
    },
    Project = new ModuleGroup
    {
        Items = [mc]
    }
};


var p = new ParserService(confog);
var parsed = p.LoadMergeContext();


var res = new MergeService(parsed);
var r = res.GetMergeResult();
var x = r;