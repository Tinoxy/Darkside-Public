package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import net.bigpoint.com.module.IModule;
   
   public class DispatchInstantCompleteRequest implements IModule
   {
      private static var _instance:DispatchInstantCompleteRequest;
      
      public var slotId:int = 0;
      
      public function DispatchInstantCompleteRequest(param1:int = 0)
      {
         super();
         this.slotId = param1;
      }
      
      public static function get instance() : DispatchInstantCompleteRequest
      {
         return _instance || (_instance = new DispatchInstantCompleteRequest());
      }
      
      public function getLibcomModuleId() : int
      {
         return 20585;
      }
      
      public function getLibcomModuleEstimatedLength() : int
      {
         return 2;
      }
      
      public function read(param1:IDataInput) : void
      {
         this.slotId = param1.readShort();
         this.slotId = 65535 & ((65535 & this.slotId) >>> 14 | (65535 & this.slotId) << 2);
         this.slotId = this.slotId > 32767 ? int(this.slotId - 65536) : this.slotId;
      }
      
      public function write(param1:IDataOutput) : void
      {
         param1.writeShort(20585);
         param1.writeShort(65535 & ((65535 & this.slotId) << 14 | (65535 & this.slotId) >>> 2));
      }
   }
}

