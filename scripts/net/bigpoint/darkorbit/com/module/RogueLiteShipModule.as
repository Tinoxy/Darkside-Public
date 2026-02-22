package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import net.bigpoint.com.module.IModule;
   
   public class RogueLiteShipModule implements IModule
   {
      private static var _instance:RogueLiteShipModule;
      
      public var equipments:Vector.<InventoryItemModule>;
      
      public var petEquipments:Vector.<InventoryItemModule>;
      
      public var spread:int = 0;
      
      public var hp:int = 0;
      
      public var speed:int = 0;
      
      public var loadOutId:String = "";
      
      public var shipId:int = 0;
      
      public var lootId:String = "";
      
      public var shield:int = 0;
      
      public var drones:Vector.<RogueLiteDroneModule>;
      
      public function RogueLiteShipModule(param1:int = 0, param2:String = "", param3:int = 0, param4:int = 0, param5:int = 0, param6:int = 0, param7:String = "", param8:Vector.<InventoryItemModule> = null, param9:Vector.<InventoryItemModule> = null, param10:Vector.<RogueLiteDroneModule> = null)
      {
         super();
         if(param8 == null)
         {
            this.equipments = new Vector.<InventoryItemModule>();
         }
         else
         {
            this.equipments = param8;
         }
         if(param9 == null)
         {
            this.petEquipments = new Vector.<InventoryItemModule>();
         }
         else
         {
            this.petEquipments = param9;
         }
         this.spread = param5;
         this.hp = param3;
         this.speed = param6;
         this.loadOutId = param2;
         this.shipId = param1;
         this.lootId = param7;
         this.shield = param4;
         if(param10 == null)
         {
            this.drones = new Vector.<RogueLiteDroneModule>();
         }
         else
         {
            this.drones = param10;
         }
      }
      
      public static function get instance() : RogueLiteShipModule
      {
         return _instance || (_instance = new RogueLiteShipModule());
      }
      
      public function getLibcomModuleId() : int
      {
         return 24289;
      }
      
      public function getLibcomModuleEstimatedLength() : int
      {
         return 36;
      }
      
      public function read(param1:IDataInput) : void
      {
         var _loc4_:InventoryItemModule = null;
         var _loc5_:InventoryItemModule = null;
         var _loc6_:RogueLiteDroneModule = null;
         var _loc2_:int = 0;
         var _loc3_:int = 0;
         while(this.equipments.length > 0)
         {
            this.equipments.pop();
         }
         _loc2_ = 0;
         _loc3_ = int(param1.readByte());
         while(_loc2_ < _loc3_)
         {
            _loc4_ = InventoryItemModule(ModuleFactory.get().createInstance(param1.readShort()));
            _loc4_.read(param1);
            this.equipments.push(_loc4_);
            _loc2_++;
         }
         while(this.petEquipments.length > 0)
         {
            this.petEquipments.pop();
         }
         _loc2_ = 0;
         _loc3_ = int(param1.readByte());
         while(_loc2_ < _loc3_)
         {
            _loc5_ = InventoryItemModule(ModuleFactory.get().createInstance(param1.readShort()));
            _loc5_.read(param1);
            this.petEquipments.push(_loc5_);
            _loc2_++;
         }
         this.spread = param1.readInt();
         this.spread = this.spread << 5 | this.spread >>> 27;
         this.hp = param1.readInt();
         this.hp = this.hp >>> 2 | this.hp << 30;
         this.speed = param1.readInt();
         this.speed = this.speed << 12 | this.speed >>> 20;
         this.loadOutId = param1.readUTF();
         this.shipId = param1.readInt();
         this.shipId = this.shipId << 13 | this.shipId >>> 19;
         this.lootId = param1.readUTF();
         this.shield = param1.readInt();
         this.shield = this.shield >>> 10 | this.shield << 22;
         while(this.drones.length > 0)
         {
            this.drones.pop();
         }
         _loc2_ = 0;
         _loc3_ = int(param1.readByte());
         while(_loc2_ < _loc3_)
         {
            _loc6_ = RogueLiteDroneModule(ModuleFactory.get().createInstance(param1.readShort()));
            _loc6_.read(param1);
            this.drones.push(_loc6_);
            _loc2_++;
         }
      }
      
      public function write(param1:IDataOutput) : void
      {
         var _loc2_:InventoryItemModule = null;
         var _loc3_:InventoryItemModule = null;
         var _loc4_:RogueLiteDroneModule = null;
         param1.writeShort(24289);
         param1.writeByte(this.equipments.length);
         for each(_loc2_ in this.equipments)
         {
            _loc2_.write(param1);
         }
         param1.writeByte(this.petEquipments.length);
         for each(_loc3_ in this.petEquipments)
         {
            _loc3_.write(param1);
         }
         param1.writeInt(this.spread >>> 5 | this.spread << 27);
         param1.writeInt(this.hp << 2 | this.hp >>> 30);
         param1.writeInt(this.speed >>> 12 | this.speed << 20);
         param1.writeUTF(this.loadOutId);
         param1.writeInt(this.shipId >>> 13 | this.shipId << 19);
         param1.writeUTF(this.lootId);
         param1.writeInt(this.shield << 10 | this.shield >>> 22);
         param1.writeByte(this.drones.length);
         for each(_loc4_ in this.drones)
         {
            _loc4_.write(param1);
         }
      }
   }
}

