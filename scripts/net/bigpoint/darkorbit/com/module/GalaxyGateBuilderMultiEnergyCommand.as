package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import net.bigpoint.com.module.IModule;
   
   public class GalaxyGateBuilderMultiEnergyCommand implements IModule
   {
      private static var _instance:GalaxyGateBuilderMultiEnergyCommand;
      
      public static const ERRORCODE_NONE:int = 0;
      
      public static const ERRORCODE_NOT_ENOUGH_MONEY:int = 1;
      
      public static const ERRORCODE_UNCLASSIFIED_ERROR:int = 2;
      
      public static const ERRORCODE__MAX:int = 3;
      
      public var samples:Number = 0;
      
      public var spinamount_selected:int = 0;
      
      public var items:Vector.<GalaxyGateBuilderItemModule>;
      
      public var gateMode:String = "";
      
      public var money:Number = 0;
      
      public var gatePartIds:Vector.<int>;
      
      public var multiplier:int = 0;
      
      public var errorCode:int = 0;
      
      public function GalaxyGateBuilderMultiEnergyCommand(param1:String = "", param2:int = 0, param3:Number = 0, param4:Number = 0, param5:int = 0, param6:Vector.<GalaxyGateBuilderItemModule> = null, param7:Vector.<int> = null, param8:int = 0)
      {
         super();
         this.samples = param4;
         this.spinamount_selected = param5;
         if(param6 == null)
         {
            this.items = new Vector.<GalaxyGateBuilderItemModule>();
         }
         else
         {
            this.items = param6;
         }
         this.gateMode = param1;
         this.money = param3;
         if(param7 == null)
         {
            this.gatePartIds = new Vector.<int>();
         }
         else
         {
            this.gatePartIds = param7;
         }
         this.multiplier = param2;
         this.errorCode = param8;
      }
      
      public static function get instance() : GalaxyGateBuilderMultiEnergyCommand
      {
         return _instance || (_instance = new GalaxyGateBuilderMultiEnergyCommand());
      }
      
      public function getLibcomModuleId() : int
      {
         return -14189;
      }
      
      public function getLibcomModuleEstimatedLength() : int
      {
         return 28;
      }
      
      public function read(param1:IDataInput) : void
      {
         var _loc4_:GalaxyGateBuilderItemModule = null;
         var _loc5_:* = 0;
         this.samples = param1.readDouble();
         this.spinamount_selected = param1.readByte();
         this.spinamount_selected = 255 & ((255 & this.spinamount_selected) << 1 | (255 & this.spinamount_selected) >>> 7);
         this.spinamount_selected = this.spinamount_selected > 127 ? int(this.spinamount_selected - 256) : this.spinamount_selected;
         var _loc2_:int = 0;
         var _loc3_:int = 0;
         while(this.items.length > 0)
         {
            this.items.pop();
         }
         _loc2_ = 0;
         _loc3_ = int(param1.readByte());
         while(_loc2_ < _loc3_)
         {
            _loc4_ = GalaxyGateBuilderItemModule(ModuleFactory.get().createInstance(param1.readShort()));
            _loc4_.read(param1);
            this.items.push(_loc4_);
            _loc2_++;
         }
         this.gateMode = param1.readUTF();
         this.money = param1.readDouble();
         while(this.gatePartIds.length > 0)
         {
            this.gatePartIds.pop();
         }
         _loc2_ = 0;
         _loc3_ = int(param1.readShort());
         while(_loc2_ < _loc3_)
         {
            _loc5_ = int(param1.readShort());
            _loc5_ = 65535 & ((65535 & _loc5_) << 10 | (65535 & _loc5_) >>> 6);
            _loc5_ = (65535 & ((65535 & _loc5_) << 10 | (65535 & _loc5_) >>> 6)) > 32767 ? int(_loc5_ - 65536) : _loc5_;
            this.gatePartIds.push(_loc5_);
            _loc2_++;
         }
         this.multiplier = param1.readByte();
         this.multiplier = 255 & ((255 & this.multiplier) << 3 | (255 & this.multiplier) >>> 5);
         this.multiplier = this.multiplier > 127 ? int(this.multiplier - 256) : this.multiplier;
         this.errorCode = param1.readShort();
      }
      
      public function write(param1:IDataOutput) : void
      {
         var _loc2_:GalaxyGateBuilderItemModule = null;
         var _loc3_:int = 0;
         param1.writeShort(-14189);
         param1.writeDouble(this.samples);
         param1.writeByte(255 & ((255 & this.spinamount_selected) >>> 1 | (255 & this.spinamount_selected) << 7));
         param1.writeByte(this.items.length);
         for each(_loc2_ in this.items)
         {
            _loc2_.write(param1);
         }
         param1.writeUTF(this.gateMode);
         param1.writeDouble(this.money);
         param1.writeShort(this.gatePartIds.length);
         for each(_loc3_ in this.gatePartIds)
         {
            param1.writeShort(65535 & ((65535 & _loc3_) >>> 10 | (65535 & _loc3_) << 6));
         }
         param1.writeByte(255 & ((255 & this.multiplier) >>> 3 | (255 & this.multiplier) << 5));
         param1.writeShort(this.errorCode);
      }
   }
}

