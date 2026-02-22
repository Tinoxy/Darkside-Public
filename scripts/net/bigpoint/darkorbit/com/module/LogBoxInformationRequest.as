package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import net.bigpoint.com.module.IModule;
   
   public class LogBoxInformationRequest implements IModule
   {
      private static var _instance:LogBoxInformationRequest;
      
      public var adler32Checksum:int = 0;
      
      public var classId:int = 0;
      
      public var typeId:int = 0;
      
      public var crc32Checksum:int = 0;
      
      public var pixelColor:int = 0;
      
      public var clipSize:String = "";
      
      public var endian:String = "";
      
      public var length:int = 0;
      
      public var quality:int = 0;
      
      public function LogBoxInformationRequest(param1:int = 0, param2:int = 0, param3:int = 0, param4:int = 0, param5:int = 0, param6:String = "", param7:int = 0, param8:String = "", param9:int = 0)
      {
         super();
         this.adler32Checksum = param4;
         this.classId = param1;
         this.typeId = param2;
         this.crc32Checksum = param3;
         this.pixelColor = param5;
         this.clipSize = param6;
         this.endian = param8;
         this.length = param9;
         this.quality = param7;
      }
      
      public static function get instance() : LogBoxInformationRequest
      {
         return _instance || (_instance = new LogBoxInformationRequest());
      }
      
      public function getLibcomModuleId() : int
      {
         return 16652;
      }
      
      public function getLibcomModuleEstimatedLength() : int
      {
         return 32;
      }
      
      public function read(param1:IDataInput) : void
      {
         this.adler32Checksum = param1.readInt();
         this.adler32Checksum = this.adler32Checksum >>> 15 | this.adler32Checksum << 17;
         this.classId = param1.readInt();
         this.classId = this.classId >>> 9 | this.classId << 23;
         this.typeId = param1.readInt();
         this.typeId = this.typeId >>> 5 | this.typeId << 27;
         this.crc32Checksum = param1.readInt();
         this.crc32Checksum = this.crc32Checksum << 16 | this.crc32Checksum >>> 16;
         this.pixelColor = param1.readInt();
         this.pixelColor = this.pixelColor >>> 10 | this.pixelColor << 22;
         this.clipSize = param1.readUTF();
         this.endian = param1.readUTF();
         this.length = param1.readInt();
         this.length = this.length << 8 | this.length >>> 24;
         this.quality = param1.readInt();
         this.quality = this.quality << 15 | this.quality >>> 17;
      }
      
      public function write(param1:IDataOutput) : void
      {
         param1.writeShort(16652);
         param1.writeInt(this.adler32Checksum << 15 | this.adler32Checksum >>> 17);
         param1.writeInt(this.classId << 9 | this.classId >>> 23);
         param1.writeInt(this.typeId << 5 | this.typeId >>> 27);
         param1.writeInt(this.crc32Checksum >>> 16 | this.crc32Checksum << 16);
         param1.writeInt(this.pixelColor << 10 | this.pixelColor >>> 22);
         param1.writeUTF(this.clipSize);
         param1.writeUTF(this.endian);
         param1.writeInt(this.length >>> 8 | this.length << 24);
         param1.writeInt(this.quality >>> 15 | this.quality << 17);
      }
   }
}

