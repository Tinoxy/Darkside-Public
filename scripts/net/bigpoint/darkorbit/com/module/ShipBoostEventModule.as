package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import net.bigpoint.com.module.IModule;
   
   public class ShipBoostEventModule implements IModule
   {
      private static var _instance:ShipBoostEventModule;
      
      public var endTime:String = "";
      
      public var attributeBoosts:Vector.<ShipBoostEventAttributeModule>;
      
      public var shipTypes:Vector.<String>;
      
      public var eventNameTextKeys:Vector.<String>;
      
      public var startTime:String = "";
      
      public var day:int = 0;
      
      public function ShipBoostEventModule(param1:int = 0, param2:String = "", param3:String = "", param4:Vector.<String> = null, param5:Vector.<ShipBoostEventAttributeModule> = null, param6:Vector.<String> = null)
      {
         super();
         this.endTime = param3;
         if(param5 == null)
         {
            this.attributeBoosts = new Vector.<ShipBoostEventAttributeModule>();
         }
         else
         {
            this.attributeBoosts = param5;
         }
         if(param4 == null)
         {
            this.shipTypes = new Vector.<String>();
         }
         else
         {
            this.shipTypes = param4;
         }
         if(param6 == null)
         {
            this.eventNameTextKeys = new Vector.<String>();
         }
         else
         {
            this.eventNameTextKeys = param6;
         }
         this.startTime = param2;
         this.day = param1;
      }
      
      public static function get instance() : ShipBoostEventModule
      {
         return _instance || (_instance = new ShipBoostEventModule());
      }
      
      public function getLibcomModuleId() : int
      {
         return -8683;
      }
      
      public function getLibcomModuleEstimatedLength() : int
      {
         return 17;
      }
      
      public function read(param1:IDataInput) : void
      {
         var _loc4_:ShipBoostEventAttributeModule = null;
         this.endTime = param1.readUTF();
         var _loc2_:int = 0;
         var _loc3_:int = 0;
         while(this.attributeBoosts.length > 0)
         {
            this.attributeBoosts.pop();
         }
         _loc2_ = 0;
         _loc3_ = int(param1.readByte());
         while(_loc2_ < _loc3_)
         {
            _loc4_ = ShipBoostEventAttributeModule(ModuleFactory.get().createInstance(param1.readShort()));
            _loc4_.read(param1);
            this.attributeBoosts.push(_loc4_);
            _loc2_++;
         }
         while(this.shipTypes.length > 0)
         {
            this.shipTypes.pop();
         }
         _loc2_ = 0;
         _loc3_ = int(param1.readByte());
         while(_loc2_ < _loc3_)
         {
            this.shipTypes.push(param1.readUTF());
            _loc2_++;
         }
         while(this.eventNameTextKeys.length > 0)
         {
            this.eventNameTextKeys.pop();
         }
         _loc2_ = 0;
         _loc3_ = int(param1.readByte());
         while(_loc2_ < _loc3_)
         {
            this.eventNameTextKeys.push(param1.readUTF());
            _loc2_++;
         }
         this.startTime = param1.readUTF();
         this.day = param1.readByte();
         this.day = 255 & ((255 & this.day) << 4 | (255 & this.day) >>> 4);
         this.day = this.day > 127 ? int(this.day - 256) : this.day;
      }
      
      public function write(param1:IDataOutput) : void
      {
         var _loc2_:ShipBoostEventAttributeModule = null;
         var _loc3_:* = null;
         var _loc4_:* = null;
         param1.writeShort(-8683);
         param1.writeUTF(this.endTime);
         param1.writeByte(this.attributeBoosts.length);
         for each(_loc2_ in this.attributeBoosts)
         {
            _loc2_.write(param1);
         }
         param1.writeByte(this.shipTypes.length);
         for each(_loc3_ in this.shipTypes)
         {
            param1.writeUTF(_loc3_);
         }
         param1.writeByte(this.eventNameTextKeys.length);
         for each(_loc4_ in this.eventNameTextKeys)
         {
            param1.writeUTF(_loc4_);
         }
         param1.writeUTF(this.startTime);
         param1.writeByte(255 & ((255 & this.day) >>> 4 | (255 & this.day) << 4));
      }
   }
}

