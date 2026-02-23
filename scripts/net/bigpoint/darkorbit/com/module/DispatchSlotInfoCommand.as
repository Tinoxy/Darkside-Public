package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import net.bigpoint.com.module.IModule;
   
   public class DispatchSlotInfoCommand implements IModule
   {
      private static var _instance:DispatchSlotInfoCommand;
      
      public var availableSlotCount:int = 0;
      
      public var currentMaxSlotCount:int = 0;
      
      public function DispatchSlotInfoCommand(param1:int = 0, param2:int = 0)
      {
         super();
         this.availableSlotCount = param1;
         this.currentMaxSlotCount = param2;
      }
      
      public static function get instance() : DispatchSlotInfoCommand
      {
         return _instance || (_instance = new DispatchSlotInfoCommand());
      }
      
      public function getLibcomModuleId() : int
      {
         return 8936;
      }
      
      public function getLibcomModuleEstimatedLength() : int
      {
         return 4;
      }
      
      public function read(param1:IDataInput) : void
      {
         this.availableSlotCount = param1.readShort();
         this.availableSlotCount = 65535 & ((65535 & this.availableSlotCount) >>> 7 | (65535 & this.availableSlotCount) << 9);
         this.availableSlotCount = this.availableSlotCount > 32767 ? int(this.availableSlotCount - 65536) : this.availableSlotCount;
         this.currentMaxSlotCount = param1.readShort();
         this.currentMaxSlotCount = 65535 & ((65535 & this.currentMaxSlotCount) << 2 | (65535 & this.currentMaxSlotCount) >>> 14);
         this.currentMaxSlotCount = this.currentMaxSlotCount > 32767 ? int(this.currentMaxSlotCount - 65536) : this.currentMaxSlotCount;
      }
      
      public function write(param1:IDataOutput) : void
      {
         param1.writeShort(8936);
         param1.writeShort(65535 & ((65535 & this.availableSlotCount) << 7 | (65535 & this.availableSlotCount) >>> 9));
         param1.writeShort(65535 & ((65535 & this.currentMaxSlotCount) >>> 2 | (65535 & this.currentMaxSlotCount) << 14));
      }
   }
}

