package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import net.bigpoint.com.module.IModule;
   
   public class GalaxyGateBuilderSaveSpinAmountRequest implements IModule
   {
      private static var _instance:GalaxyGateBuilderSaveSpinAmountRequest;
      
      public var spinAmount:int = 0;
      
      public function GalaxyGateBuilderSaveSpinAmountRequest(param1:int = 0)
      {
         super();
         this.spinAmount = param1;
      }
      
      public static function get instance() : GalaxyGateBuilderSaveSpinAmountRequest
      {
         return _instance || (_instance = new GalaxyGateBuilderSaveSpinAmountRequest());
      }
      
      public function getLibcomModuleId() : int
      {
         return 29446;
      }
      
      public function getLibcomModuleEstimatedLength() : int
      {
         return 1;
      }
      
      public function read(param1:IDataInput) : void
      {
         this.spinAmount = param1.readByte();
         this.spinAmount = 255 & ((255 & this.spinAmount) << 0 | (255 & this.spinAmount) >>> 8);
         this.spinAmount = this.spinAmount > 127 ? int(this.spinAmount - 256) : this.spinAmount;
      }
      
      public function write(param1:IDataOutput) : void
      {
         param1.writeShort(29446);
         param1.writeByte(255 & ((255 & this.spinAmount) >>> 0 | (255 & this.spinAmount) << 8));
      }
   }
}

