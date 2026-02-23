package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import net.bigpoint.com.module.IModule;
   
   public class QuantumFluidCoreData implements IModule
   {
      private static var _instance:QuantumFluidCoreData;
      
      public var shipIds:Vector.<String>;
      
      public var equipAttributes:Vector.<EquipAttributeModule>;
      
      public var spendPrice:int = 0;
      
      public var quality:int = 0;
      
      public var itemId:Number = 0;
      
      public var maxLv:int = 0;
      
      public var useLootId:String = "";
      
      public var unlock:int = 0;
      
      public var lootId:String = "";
      
      public var lv:int = 0;
      
      public var sellPrice:int = 0;
      
      public function QuantumFluidCoreData(param1:String = "", param2:Number = 0, param3:int = 0, param4:int = 0, param5:int = 0, param6:Vector.<String> = null, param7:Vector.<EquipAttributeModule> = null, param8:String = "", param9:int = 0, param10:int = 0, param11:int = 0)
      {
         super();
         if(param6 == null)
         {
            this.shipIds = new Vector.<String>();
         }
         else
         {
            this.shipIds = param6;
         }
         if(param7 == null)
         {
            this.equipAttributes = new Vector.<EquipAttributeModule>();
         }
         else
         {
            this.equipAttributes = param7;
         }
         this.spendPrice = param10;
         this.quality = param5;
         this.itemId = param2;
         this.maxLv = param4;
         this.useLootId = param8;
         this.unlock = param11;
         this.lootId = param1;
         this.lv = param3;
         this.sellPrice = param9;
      }
      
      public static function get instance() : QuantumFluidCoreData
      {
         return _instance || (_instance = new QuantumFluidCoreData());
      }
      
      public function getLibcomModuleId() : int
      {
         return -13763;
      }
      
      public function getLibcomModuleEstimatedLength() : int
      {
         return 44;
      }
      
      public function read(param1:IDataInput) : void
      {
         var _loc4_:EquipAttributeModule = null;
         var _loc2_:int = 0;
         var _loc3_:int = 0;
         while(this.shipIds.length > 0)
         {
            this.shipIds.pop();
         }
         _loc2_ = 0;
         _loc3_ = int(param1.readByte());
         while(_loc2_ < _loc3_)
         {
            this.shipIds.push(param1.readUTF());
            _loc2_++;
         }
         while(this.equipAttributes.length > 0)
         {
            this.equipAttributes.pop();
         }
         _loc2_ = 0;
         _loc3_ = int(param1.readByte());
         while(_loc2_ < _loc3_)
         {
            _loc4_ = EquipAttributeModule(ModuleFactory.get().createInstance(param1.readShort()));
            _loc4_.read(param1);
            this.equipAttributes.push(_loc4_);
            _loc2_++;
         }
         this.spendPrice = param1.readInt();
         this.spendPrice = this.spendPrice << 10 | this.spendPrice >>> 22;
         this.quality = param1.readInt();
         this.quality = this.quality << 10 | this.quality >>> 22;
         this.itemId = param1.readDouble();
         this.maxLv = param1.readInt();
         this.maxLv = this.maxLv << 9 | this.maxLv >>> 23;
         this.useLootId = param1.readUTF();
         this.unlock = param1.readInt();
         this.unlock = this.unlock >>> 14 | this.unlock << 18;
         this.lootId = param1.readUTF();
         this.lv = param1.readInt();
         this.lv = this.lv >>> 6 | this.lv << 26;
         this.sellPrice = param1.readInt();
         this.sellPrice = this.sellPrice >>> 13 | this.sellPrice << 19;
      }
      
      public function write(param1:IDataOutput) : void
      {
         var _loc2_:* = null;
         var _loc3_:EquipAttributeModule = null;
         param1.writeShort(-13763);
         param1.writeByte(this.shipIds.length);
         for each(_loc2_ in this.shipIds)
         {
            param1.writeUTF(_loc2_);
         }
         param1.writeByte(this.equipAttributes.length);
         for each(_loc3_ in this.equipAttributes)
         {
            _loc3_.write(param1);
         }
         param1.writeInt(this.spendPrice >>> 10 | this.spendPrice << 22);
         param1.writeInt(this.quality >>> 10 | this.quality << 22);
         param1.writeDouble(this.itemId);
         param1.writeInt(this.maxLv >>> 9 | this.maxLv << 23);
         param1.writeUTF(this.useLootId);
         param1.writeInt(this.unlock << 14 | this.unlock >>> 18);
         param1.writeUTF(this.lootId);
         param1.writeInt(this.lv << 6 | this.lv >>> 26);
         param1.writeInt(this.sellPrice << 13 | this.sellPrice >>> 19);
      }
   }
}

