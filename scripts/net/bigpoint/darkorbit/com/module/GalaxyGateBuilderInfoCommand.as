package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import net.bigpoint.com.module.IModule;
   
   public class GalaxyGateBuilderInfoCommand implements IModule
   {
      private static var _instance:GalaxyGateBuilderInfoCommand;
      
      public var bonusRewardsDay:Boolean = false;
      
      public var samples:Number = 0;
      
      public var spinamount_selected:int = 0;
      
      public var gates:Vector.<GalaxyGateBuilderGateModule>;
      
      public var energy_cost:int = 0;
      
      public var money:Number = 0;
      
      public var galaxyGateDay:Boolean = false;
      
      public var spinAmounts:Vector.<int>;
      
      public var spinOnSale:Boolean = false;
      
      public function GalaxyGateBuilderInfoCommand(param1:Number = 0, param2:Number = 0, param3:int = 0, param4:int = 0, param5:Boolean = false, param6:Boolean = false, param7:Boolean = false, param8:Vector.<int> = null, param9:Vector.<GalaxyGateBuilderGateModule> = null)
      {
         super();
         this.bonusRewardsDay = param7;
         this.samples = param2;
         this.spinamount_selected = param4;
         if(param9 == null)
         {
            this.gates = new Vector.<GalaxyGateBuilderGateModule>();
         }
         else
         {
            this.gates = param9;
         }
         this.energy_cost = param3;
         this.money = param1;
         this.galaxyGateDay = param6;
         if(param8 == null)
         {
            this.spinAmounts = new Vector.<int>();
         }
         else
         {
            this.spinAmounts = param8;
         }
         this.spinOnSale = param5;
      }
      
      public static function get instance() : GalaxyGateBuilderInfoCommand
      {
         return _instance || (_instance = new GalaxyGateBuilderInfoCommand());
      }
      
      public function getLibcomModuleId() : int
      {
         return 526;
      }
      
      public function getLibcomModuleEstimatedLength() : int
      {
         return 30;
      }
      
      public function read(param1:IDataInput) : void
      {
         var _loc4_:GalaxyGateBuilderGateModule = null;
         var _loc5_:* = 0;
         this.bonusRewardsDay = param1.readBoolean();
         this.samples = param1.readDouble();
         this.spinamount_selected = param1.readByte();
         this.spinamount_selected = 255 & ((255 & this.spinamount_selected) << 7 | (255 & this.spinamount_selected) >>> 1);
         this.spinamount_selected = this.spinamount_selected > 127 ? int(this.spinamount_selected - 256) : this.spinamount_selected;
         var _loc2_:int = 0;
         var _loc3_:int = 0;
         while(this.gates.length > 0)
         {
            this.gates.pop();
         }
         _loc2_ = 0;
         _loc3_ = int(param1.readByte());
         while(_loc2_ < _loc3_)
         {
            _loc4_ = GalaxyGateBuilderGateModule(ModuleFactory.get().createInstance(param1.readShort()));
            _loc4_.read(param1);
            this.gates.push(_loc4_);
            _loc2_++;
         }
         this.energy_cost = param1.readShort();
         this.energy_cost = 65535 & ((65535 & this.energy_cost) << 13 | (65535 & this.energy_cost) >>> 3);
         this.energy_cost = this.energy_cost > 32767 ? int(this.energy_cost - 65536) : this.energy_cost;
         this.money = param1.readDouble();
         this.galaxyGateDay = param1.readBoolean();
         while(this.spinAmounts.length > 0)
         {
            this.spinAmounts.pop();
         }
         _loc2_ = 0;
         _loc3_ = int(param1.readByte());
         while(_loc2_ < _loc3_)
         {
            _loc5_ = int(param1.readByte());
            _loc5_ = 255 & ((255 & _loc5_) >>> 5 | (255 & _loc5_) << 3);
            _loc5_ = (255 & ((255 & _loc5_) >>> 5 | (255 & _loc5_) << 3)) > 127 ? int(_loc5_ - 256) : _loc5_;
            this.spinAmounts.push(_loc5_);
            _loc2_++;
         }
         this.spinOnSale = param1.readBoolean();
      }
      
      public function write(param1:IDataOutput) : void
      {
         var _loc2_:GalaxyGateBuilderGateModule = null;
         var _loc3_:int = 0;
         param1.writeShort(526);
         param1.writeBoolean(this.bonusRewardsDay);
         param1.writeDouble(this.samples);
         param1.writeByte(255 & ((255 & this.spinamount_selected) >>> 7 | (255 & this.spinamount_selected) << 1));
         param1.writeByte(this.gates.length);
         for each(_loc2_ in this.gates)
         {
            _loc2_.write(param1);
         }
         param1.writeShort(65535 & ((65535 & this.energy_cost) >>> 13 | (65535 & this.energy_cost) << 3));
         param1.writeDouble(this.money);
         param1.writeBoolean(this.galaxyGateDay);
         param1.writeByte(this.spinAmounts.length);
         for each(_loc3_ in this.spinAmounts)
         {
            param1.writeByte(255 & ((255 & _loc3_) << 5 | (255 & _loc3_) >>> 3));
         }
         param1.writeBoolean(this.spinOnSale);
      }
   }
}

