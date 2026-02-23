package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import net.bigpoint.com.module.IModule;
   
   public class DispatchSendGateRequest implements IModule
   {
      private static var _instance:DispatchSendGateRequest;
      
      public var gateId:int = 0;
      
      public function DispatchSendGateRequest(param1:int = 0)
      {
         super();
         this.gateId = param1;
      }
      
      public static function get instance() : DispatchSendGateRequest
      {
         return _instance || (_instance = new DispatchSendGateRequest());
      }
      
      public function getLibcomModuleId() : int
      {
         return 28603;
      }
      
      public function getLibcomModuleEstimatedLength() : int
      {
         return 2;
      }
      
      public function read(param1:IDataInput) : void
      {
         this.gateId = param1.readShort();
         this.gateId = 65535 & ((65535 & this.gateId) << 1 | (65535 & this.gateId) >>> 15);
         this.gateId = this.gateId > 32767 ? int(this.gateId - 65536) : this.gateId;
      }
      
      public function write(param1:IDataOutput) : void
      {
         param1.writeShort(28603);
         param1.writeShort(65535 & ((65535 & this.gateId) >>> 1 | (65535 & this.gateId) << 15));
      }
   }
}

