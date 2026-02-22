package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import net.bigpoint.com.module.IModule;
   
   public class GalaxyGateBuilderMultiEnergyRequest implements IModule
   {
      private static var _instance:GalaxyGateBuilderMultiEnergyRequest;
      
      public var useMultiplier:Boolean = false;
      
      public var gateId:int = 0;
      
      public var spinAmount:int = 0;
      
      public function GalaxyGateBuilderMultiEnergyRequest(param1:int = 0, param2:Boolean = false, param3:int = 0)
      {
         super();
         this.useMultiplier = param2;
         this.gateId = param1;
         this.spinAmount = param3;
      }
      
      public static function get instance() : GalaxyGateBuilderMultiEnergyRequest
      {
         return _instance || (_instance = new GalaxyGateBuilderMultiEnergyRequest());
      }
      
      public function getLibcomModuleId() : int
      {
         return -14674;
      }
      
      public function getLibcomModuleEstimatedLength() : int
      {
         return 3;
      }
      
      public function read(param1:IDataInput) : void
      {
         this.useMultiplier = param1.readBoolean();
         this.gateId = param1.readByte();
         this.gateId = 255 & ((255 & this.gateId) << 3 | (255 & this.gateId) >>> 5);
         this.gateId = this.gateId > 127 ? int(this.gateId - 256) : this.gateId;
         this.spinAmount = param1.readByte();
         this.spinAmount = 255 & ((255 & this.spinAmount) >>> 5 | (255 & this.spinAmount) << 3);
         this.spinAmount = this.spinAmount > 127 ? int(this.spinAmount - 256) : this.spinAmount;
      }
      
      public function write(param1:IDataOutput) : void
      {
         param1.writeShort(-14674);
         param1.writeBoolean(this.useMultiplier);
         param1.writeByte(255 & ((255 & this.gateId) >>> 3 | (255 & this.gateId) << 5));
         param1.writeByte(255 & ((255 & this.spinAmount) << 5 | (255 & this.spinAmount) >>> 3));
      }
   }
}

