package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import net.bigpoint.com.module.IModule;
   
   public class EventModule implements IModule
   {
      private static var _instance:EventModule;
      
      public var lang:String = "";
      
      public var id:Number = 0;
      
      public var type:String = "";
      
      public var timeLeft:int = 0;
      
      public function EventModule(param1:String = "", param2:String = "", param3:Number = 0, param4:int = 0)
      {
         super();
         this.lang = param2;
         this.id = param3;
         this.type = param1;
         this.timeLeft = param4;
      }
      
      public static function get instance() : EventModule
      {
         return _instance || (_instance = new EventModule());
      }
      
      public function getLibcomModuleId() : int
      {
         return 30608;
      }
      
      public function getLibcomModuleEstimatedLength() : int
      {
         return 16;
      }
      
      public function read(param1:IDataInput) : void
      {
         this.lang = param1.readUTF();
         this.id = param1.readDouble();
         this.type = param1.readUTF();
         this.timeLeft = param1.readInt();
         this.timeLeft = this.timeLeft << 11 | this.timeLeft >>> 21;
      }
      
      public function write(param1:IDataOutput) : void
      {
         param1.writeShort(30608);
         param1.writeUTF(this.lang);
         param1.writeDouble(this.id);
         param1.writeUTF(this.type);
         param1.writeInt(this.timeLeft >>> 11 | this.timeLeft << 21);
      }
   }
}

