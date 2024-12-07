using System;
using System.IO;
using System.Runtime.CompilerServices;
using ZXing;
using ZXing.QrCode;

var first_use_message_file = "首次使用請在 data.txt 中輸入 QR Code 的內容";

var content = string.Empty;
if (File.Exists("data.txt"))
{
   using var file = new StreamReader("data.txt");
   content = file.ReadToEnd();

   if (File.Exists(first_use_message_file))
      File.Delete(first_use_message_file);
}
else
{
   Console.WriteLine("首次使用請先輸入 QR Code 的內容，之後直接在 data 文字檔輸入就行");
   Console.Write("請輸入 QR Code 的內容：");

   read_console_line();
   switch (content)
   {
      case "":
         reply();
         break;
      case null:
         first_use();
         return;
   }
   first_save();
}

var writer = new BarcodeWriterSvg
{
   Format = BarcodeFormat.QR_CODE,
   Options = new QrCodeEncodingOptions
   {
      Margin = 1,
      Width = 2000,
      Height = 2000,
      CharacterSet = "UTF-8",
   }
};
var img_data = writer.Write(content);
using (var img = new StreamWriter("qr_cord.svg"))
{
   img.Write(img_data);
}

[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
void read_console_line()
{
   content = Console.ReadLine();
}

void first_use()
{
   File.Create(first_use_message_file);
   content = "請輸入 QR Code 的內容";
   first_save();
}

void first_save()
{
   using var file = new StreamWriter("data.txt");
   file.Write(content);
}

void reply()
{
   do
   {
      Console.Write("沒有輸入東西，請重新輸入：");
      read_console_line();
   }
   while (content.Equals(string.Empty));
}