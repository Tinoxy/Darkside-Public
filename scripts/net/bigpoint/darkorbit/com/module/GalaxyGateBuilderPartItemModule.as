package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   
   public class GalaxyGateBuilderPartItemModule extends GalaxyGateBuilderItemModule
   {
      private static var _instance:GalaxyGateBuilderPartItemModule;
      
      public static const GATESTATE_IN_PROGRESS:int = 0;
      
      public static const GATESTATE_FINISHED:int = 1;
      
      public static const GATESTATE__MAX:int = 2;
      
      public var lifePrice:int = 0;
      
      public var part_id:int = 0;
      
      public var livesLeft:int = 0;
      
      public var multiplier_used:int = 0;
      
      public var state:int = 0;
      
      public var gate_id:int = 0;
      
      public var current:int = 0;
      
      public var duplicate:Boolean = false;
      
      public function GalaxyGateBuilderPartItemModule(param1:int = 0, param2:Number = 0, param3:Boolean = false, param4:int = 0, param5:int = 0, param6:int = 0, param7:int = 0, param8:int = 0, param9:int = 0, param10:int = 0)
      {
         super(param1,param2);
         this.lifePrice = param9;
         this.part_id = param5;
         this.livesLeft = param8;
         this.multiplier_used = param10;
         this.state = param6;
         this.gate_id = param4;
         this.current = param7;
         this.duplicate = param3;
      }
      
      public static function get instance() : GalaxyGateBuilderPartItemModule
      {
         return _instance || (_instance = new GalaxyGateBuilderPartItemModule());
      }
      
      override public function getLibcomModuleId() : int
      {
         return 31292;
      }
      
      override public function getLibcomModuleEstimatedLength() : int
      {
         return 15;
      }
      
      override public function read(param1:IDataInput) : void
      {
         param1.readShort();
         super.read(param1);
         this.lifePrice = param1.readInt();
         this.lifePrice = this.lifePrice << 5 | this.lifePrice >>> 27;
         this.part_id = param1.readShort();
         this.part_id = 65535 & ((65535 & this.part_id) << 7 | (65535 & this.part_id) >>> 9);
         this.part_id = this.part_id > 32767 ? int(this.part_id - 65536) : this.part_id;
         this.livesLeft = param1.readInt();
         this.livesLeft = this.livesLeft << 8 | this.livesLeft >>> 24;
         this.multiplier_used = param1.readByte();
         this.multiplier_used = 255 & ((255 & this.multiplier_used) >>> 6 | (255 & this.multiplier_used) << 2);
         this.multiplier_used = this.multiplier_used > 127 ? int(this.multiplier_used - 256) : this.multiplier_used;
         this.state = param1.readShort();
         this.gate_id = param1.readByte();
         this.gate_id = 255 & ((255 & this.gate_id) << 6 | (255 & this.gate_id) >>> 2);
         this.gate_id = this.gate_id > 127 ? int(this.gate_id - 256) : this.gate_id;
         this.current = param1.readShort();
         this.current = 65535 & ((65535 & this.current) << 11 | (65535 & this.current) >>> 5);
         this.current = this.current > 32767 ? int(this.current - 65536) : this.current;
         this.duplicate = param1.readBoolean();
      }
      
      override public function write(param1:IDataOutput) : void
      {
         param1.writeShort(31292);
         super.write(param1);
         param1.writeInt(this.lifePrice >>> 5 | this.lifePrice << 27);
         param1.writeShort(65535 & ((65535 & this.part_id) >>> 7 | (65535 & this.part_id) << 9));
         param1.writeInt(this.livesLeft >>> 8 | this.livesLeft << 24);
         param1.writeByte(255 & ((255 & this.multiplier_used) << 6 | (255 & this.multiplier_used) >>> 2));
         param1.writeShort(this.state);
         param1.writeByte(255 & ((255 & this.gate_id) >>> 6 | (255 & this.gate_id) << 2));
         param1.writeShort(65535 & ((65535 & this.current) >>> 11 | (65535 & this.current) << 5));
         param1.writeBoolean(this.duplicate);
      }
   }
}

