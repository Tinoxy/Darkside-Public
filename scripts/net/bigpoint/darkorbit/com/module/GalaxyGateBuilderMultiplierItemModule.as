package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   
   public class GalaxyGateBuilderMultiplierItemModule extends GalaxyGateBuilderItemModule
   {
      private static var _instance:GalaxyGateBuilderMultiplierItemModule;
      
      public var amount:int = 0;
      
      public function GalaxyGateBuilderMultiplierItemModule(param1:int = 0, param2:Number = 0, param3:int = 0)
      {
         super(param1,param2);
         this.amount = param3;
      }
      
      public static function get instance() : GalaxyGateBuilderMultiplierItemModule
      {
         return _instance || (_instance = new GalaxyGateBuilderMultiplierItemModule());
      }
      
      override public function getLibcomModuleId() : int
      {
         return 6029;
      }
      
      override public function getLibcomModuleEstimatedLength() : int
      {
         return 1;
      }
      
      override public function read(param1:IDataInput) : void
      {
         param1.readShort();
         super.read(param1);
         this.amount = param1.readByte();
         this.amount = 255 & ((255 & this.amount) >>> 6 | (255 & this.amount) << 2);
         this.amount = this.amount > 127 ? int(this.amount - 256) : this.amount;
      }
      
      override public function write(param1:IDataOutput) : void
      {
         param1.writeShort(6029);
         super.write(param1);
         param1.writeByte(255 & ((255 & this.amount) << 6 | (255 & this.amount) >>> 2));
      }
   }
}

