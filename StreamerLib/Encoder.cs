using Gst;
using System;

namespace StreamerLib;
public class Encoder : IEncoder
    {
        public void EncodeData(string inputPath, string outputPath)
        {
            try
            {
                // Initialize GStreamer
                Application.Init();

                // Create the GStreamer encoding pipeline description
                string pipelineDescription = $@"
                    filesrc location={inputPath} ! decodebin !
                    videoconvert ! x264enc tune=zerolatency ! mp4mux !
                    filesink location={outputPath}";


                // Create the pipeline
                using var pipeline = Parse.Launch(pipelineDescription) as Pipeline;

                if (pipeline == null)
                {
                    Console.WriteLine("Failed to create GStreamer pipeline.");
                    return;
                }

                // Attach to the bus for message handling (errors, EOS)
                var bus = pipeline.Bus;
                bus.AddWatch(OnBusMessage);

                // Start the pipeline
                pipeline.SetState(State.Playing);

                // Wait for the pipeline to finish
                //pipeline.Wait();

                Console.WriteLine("Encoding completed successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred during encoding: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
            }
        }

        private bool OnBusMessage(Bus bus, Message msg)
        {
            switch (msg.Type)
            {
                case MessageType.Eos:
                    Console.WriteLine("End of stream reached.");
                    break;

                case MessageType.Error:
                    msg.ParseError(out GLib.GException error, out string debug);
                    Console.WriteLine($"Error: {error.Message}");
                    if (debug != null) Console.WriteLine($"Debug info: {debug}");
                    break;
            }
            return true;
        }
    }

