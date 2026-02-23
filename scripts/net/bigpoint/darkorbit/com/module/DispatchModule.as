package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import net.bigpoint.com.module.IModule;
   
   public class DispatchModule implements IModule
   {
      private static var _instance:DispatchModule;
      
      public var slotId:int = 0;
      
      public var id:int = 0;
      
      public var duration:Number = 0;
      
      public function DispatchModule(param1:int = 0, param2:Number = 0, param3:int = 0)
      {
         super();
         this.slotId = param3;
         this.id = param1;
         this.duration = param2;
      }
      
      public static function get instance() : DispatchModule
      {
         return _instance || (_instance = new DispatchModule());
      }
      
      public function getLibcomModuleId() : int
      {
         return -31536;
      }
      
      public function getLibcomModuleEstimatedLength() : int
      {
         return 12;
      }
      
      public function read(param1:IDataInput) : void
      {
         this.slotId = param1.readShort();
         this.slotId = 65535 & ((65535 & this.slotId) >>> 5 | (65535 & this.slotId) << 11);
         this.slotId = this.slotId > 32767 ? int(this.slotId - 65536) : this.slotId;
         this.id = param1.readShort();
         this.id = 65535 & ((65535 & this.id) << 14 | (65535 & this.id) >>> 2);
         this.id = this.id > 32767 ? int(this.id - 65536) : this.id;
         this.duration = param1.readDouble();
      }
      
      public function write(param1:IDataOutput) : void
      {
         param1.writeShort(-31536);
         param1.writeShort(65535 & ((65535 & this.slotId) << 5 | (65535 & this.slotId) >>> 11));
         param1.writeShort(65535 & ((65535 & this.id) >>> 14 | (65535 & this.id) << 2));
         param1.writeDouble(this.duration);
      }
   }
}

