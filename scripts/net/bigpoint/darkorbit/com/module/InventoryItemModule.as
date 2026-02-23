package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import net.bigpoint.com.module.IModule;
   
   public class InventoryItemModule implements IModule
   {
      private static var _instance:InventoryItemModule;
      
      public var upgradeLevel:int = 0;
      
      public var amount:Number = 0;
      
      public var hangarId:Number = 0;
      
      public var lootId:String = "";
      
      public var isNew:Boolean = false;
      
      public var itemId:Number = 0;
      
      public var inventoryItemAttributes:Vector.<InventoryItemAttributeModule>;
      
      public function InventoryItemModule(param1:Number = 0, param2:String = "", param3:Number = 0, param4:int = 0, param5:Boolean = false, param6:Number = 0, param7:Vector.<InventoryItemAttributeModule> = null)
      {
         super();
         this.upgradeLevel = param4;
         this.amount = param3;
         this.hangarId = param6;
         this.lootId = param2;
         this.isNew = param5;
         this.itemId = param1;
         if(param7 == null)
         {
            this.inventoryItemAttributes = new Vector.<InventoryItemAttributeModule>();
         }
         else
         {
            this.inventoryItemAttributes = param7;
         }
      }
      
      public static function get instance() : InventoryItemModule
      {
         return _instance || (_instance = new InventoryItemModule());
      }
      
      public function getLibcomModuleId() : int
      {
         return -12971;
      }
      
      public function getLibcomModuleEstimatedLength() : int
      {
         return 35;
      }
      
      public function read(param1:IDataInput) : void
      {
         var _loc4_:InventoryItemAttributeModule = null;
         this.upgradeLevel = param1.readInt();
         this.upgradeLevel = this.upgradeLevel << 8 | this.upgradeLevel >>> 24;
         this.amount = param1.readDouble();
         this.hangarId = param1.readDouble();
         this.lootId = param1.readUTF();
         this.isNew = param1.readBoolean();
         this.itemId = param1.readDouble();
         var _loc2_:int = 0;
         var _loc3_:int = 0;
         while(this.inventoryItemAttributes.length > 0)
         {
            this.inventoryItemAttributes.pop();
         }
         _loc2_ = 0;
         _loc3_ = int(param1.readByte());
         while(_loc2_ < _loc3_)
         {
            _loc4_ = InventoryItemAttributeModule(ModuleFactory.get().createInstance(param1.readShort()));
            _loc4_.read(param1);
            this.inventoryItemAttributes.push(_loc4_);
            _loc2_++;
         }
      }
      
      public function write(param1:IDataOutput) : void
      {
         var _loc2_:InventoryItemAttributeModule = null;
         param1.writeShort(-12971);
         param1.writeInt(this.upgradeLevel >>> 8 | this.upgradeLevel << 24);
         param1.writeDouble(this.amount);
         param1.writeDouble(this.hangarId);
         param1.writeUTF(this.lootId);
         param1.writeBoolean(this.isNew);
         param1.writeDouble(this.itemId);
         param1.writeByte(this.inventoryItemAttributes.length);
         for each(_loc2_ in this.inventoryItemAttributes)
         {
            _loc2_.write(param1);
         }
      }
   }
}

