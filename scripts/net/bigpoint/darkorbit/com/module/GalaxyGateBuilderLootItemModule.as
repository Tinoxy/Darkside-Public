package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   
   public class GalaxyGateBuilderLootItemModule extends GalaxyGateBuilderItemModule
   {
      private static var _instance:GalaxyGateBuilderLootItemModule;
      
      public var amount:int = 0;
      
      public var multiplier_used:int = 0;
      
      public var multiplier_amount:int = 0;
      
      public var item_id:int = 0;
      
      public var spins:int = 0;
      
      public function GalaxyGateBuilderLootItemModule(param1:int = 0, param2:Number = 0, param3:int = 0, param4:int = 0, param5:int = 0, param6:int = 0, param7:int = 0)
      {
         super(param1,param2);
         this.amount = param4;
         this.multiplier_used = param6;
         this.multiplier_amount = param5;
         this.item_id = param3;
         this.spins = param7;
      }
      
      public static function get instance() : GalaxyGateBuilderLootItemModule
      {
         return _instance || (_instance = new GalaxyGateBuilderLootItemModule());
      }
      
      override public function getLibcomModuleId() : int
      {
         return 14665;
      }
      
      override public function getLibcomModuleEstimatedLength() : int
      {
         return 12;
      }
      
      override public function read(param1:IDataInput) : void
      {
         param1.readShort();
         super.read(param1);
         this.amount = param1.readInt();
         this.amount = this.amount << 10 | this.amount >>> 22;
         this.multiplier_used = param1.readShort();
         this.multiplier_used = 65535 & ((65535 & this.multiplier_used) >>> 12 | (65535 & this.multiplier_used) << 4);
         this.multiplier_used = this.multiplier_used > 32767 ? int(this.multiplier_used - 65536) : this.multiplier_used;
         this.multiplier_amount = param1.readInt();
         this.multiplier_amount = this.multiplier_amount >>> 3 | this.multiplier_amount << 29;
         this.item_id = param1.readByte();
         this.item_id = 255 & ((255 & this.item_id) >>> 3 | (255 & this.item_id) << 5);
         this.item_id = this.item_id > 127 ? int(this.item_id - 256) : this.item_id;
         this.spins = param1.readByte();
         this.spins = 255 & ((255 & this.spins) << 6 | (255 & this.spins) >>> 2);
         this.spins = this.spins > 127 ? int(this.spins - 256) : this.spins;
      }
      
      override public function write(param1:IDataOutput) : void
      {
         param1.writeShort(14665);
         super.write(param1);
         param1.writeInt(this.amount >>> 10 | this.amount << 22);
         param1.writeShort(65535 & ((65535 & this.multiplier_used) << 12 | (65535 & this.multiplier_used) >>> 4));
         param1.writeInt(this.multiplier_amount << 3 | this.multiplier_amount >>> 29);
         param1.writeByte(255 & ((255 & this.item_id) << 3 | (255 & this.item_id) >>> 5));
         param1.writeByte(255 & ((255 & this.spins) >>> 6 | (255 & this.spins) << 2));
      }
   }
}

