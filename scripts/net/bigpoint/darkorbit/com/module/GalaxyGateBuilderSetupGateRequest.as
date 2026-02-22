package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import net.bigpoint.com.module.IModule;
   
   public class GalaxyGateBuilderSetupGateRequest implements IModule
   {
      private static var _instance:GalaxyGateBuilderSetupGateRequest;
      
      public var gateId:int = 0;
      
      public function GalaxyGateBuilderSetupGateRequest(param1:int = 0)
      {
         super();
         this.gateId = param1;
      }
      
      public static function get instance() : GalaxyGateBuilderSetupGateRequest
      {
         return _instance || (_instance = new GalaxyGateBuilderSetupGateRequest());
      }
      
      public function getLibcomModuleId() : int
      {
         return 25136;
      }
      
      public function getLibcomModuleEstimatedLength() : int
      {
         return 1;
      }
      
      public function read(param1:IDataInput) : void
      {
         this.gateId = param1.readByte();
         this.gateId = 255 & ((255 & this.gateId) >>> 4 | (255 & this.gateId) << 4);
         this.gateId = this.gateId > 127 ? int(this.gateId - 256) : this.gateId;
      }
      
      public function write(param1:IDataOutput) : void
      {
         param1.writeShort(25136);
         param1.writeByte(255 & ((255 & this.gateId) << 4 | (255 & this.gateId) >>> 4));
      }
   }
}

