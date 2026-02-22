package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import net.bigpoint.com.module.IModule;
   
   public class DispatchCollectGateRewardRequest implements IModule
   {
      private static var _instance:DispatchCollectGateRewardRequest;
      
      public var gateId:int = 0;
      
      public function DispatchCollectGateRewardRequest(param1:int = 0)
      {
         super();
         this.gateId = param1;
      }
      
      public static function get instance() : DispatchCollectGateRewardRequest
      {
         return _instance || (_instance = new DispatchCollectGateRewardRequest());
      }
      
      public function getLibcomModuleId() : int
      {
         return -28510;
      }
      
      public function getLibcomModuleEstimatedLength() : int
      {
         return 2;
      }
      
      public function read(param1:IDataInput) : void
      {
         this.gateId = param1.readShort();
         this.gateId = 65535 & ((65535 & this.gateId) >>> 1 | (65535 & this.gateId) << 15);
         this.gateId = this.gateId > 32767 ? int(this.gateId - 65536) : this.gateId;
      }
      
      public function write(param1:IDataOutput) : void
      {
         param1.writeShort(-28510);
         param1.writeShort(65535 & ((65535 & this.gateId) << 1 | (65535 & this.gateId) >>> 15));
      }
   }
}

