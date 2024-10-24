// See https://aka.ms/new-console-template for more information
using Gst;
using StreamerLib;

var p = new Encoder();
try
{
    p.EncodeData("/home/benji/Desktop/test.mp4", "/home/benji/Desktop/test1.mp4");
}
catch (Exception e)
{
    Console.WriteLine(e);
    throw;
}
