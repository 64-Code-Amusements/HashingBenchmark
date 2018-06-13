// aLearn more about F# at http://fsharp.org

open System
open System.Security.Cryptography
open System.Text
open System.IO
open System.Diagnostics

let hash (hashAlgorithm:HashAlgorithm) (stream:Stream) =
  (StringBuilder(), hashAlgorithm.ComputeHash(stream))
  ||> Array.fold(fun sb b -> sb.Append(b.ToString("x2")))
  |> string

let hashWithMD5 stream =
  let hashAlgorithm = MD5.Create()
  hash hashAlgorithm stream
   
let hashWithSHA1 stream =
  let hashAlgorithm = SHA1.Create()
  hash hashAlgorithm stream
   
let hashWithSHA256 stream =
  let hashAlgorithm = SHA256.Create()
  hash hashAlgorithm stream
   
let processFiles (algorithm:HashAlgorithm) filenames =
  filenames |> Array.map (fun f ->
          let fileInfo = FileInfo(f)
          let stopWatch = new Stopwatch()
          stopWatch.Start()
          let hash =  File.OpenRead(f) |> algorithm
          stopWatch.Stop()
          [ hash fileInfo.Name fileInfo.Length stopWatch.ElapsedMilliseconds]
          )

[<EntryPoint>]
let main argv =
  let filenames =[|
      "d:\\temp\\data\\300kinvoices.imd";
      "C:\\Program Files (x86)\\Microsoft SDKs\\Windows Phone\\v8.1\\Emulation\\Images\\flash.vhd";
      "C:\\Program Files (x86)\\Microsoft SDKs\\Windows Phone\\v8.0\\Emulation\\Images\\Flash.vhd";
      "D:\\temp\\Sunder_Parking\\2008-2015_Top10Parking.IMD";
      "c:\\temp\\2008-2015_Top10Parking.IMD";
      "D:\\Downloads\\Software to install\\Visual_Studio_2017_Offline_Installer.iso"
  |]
  let algorithms = [|
    hashWithMD5;
    hashWithSHA1;
    hashWithSHA256
  |]


  let result = algorithms |> Array.map (fun algorithm -> filenames |> processFiles algorithm )
  printfn "%A" result
  Console.ReadKey() |> ignore
  0 // return an integer exit code
