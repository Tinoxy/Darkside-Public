package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import net.bigpoint.com.module.IModule;
   
   public class ShipUpgradeModuleUpdateCommand implements IModule
   {
      private static var _instance:ShipUpgradeModuleUpdateCommand;
      
      public var moduleTypeIds:Vector.<int>;
      
      public var uid:int = 0;
      
      public var max:int = 0;
      
      public function ShipUpgradeModuleUpdateCommand(param1:int = 0, param2:Vector.<int> = null, param3:int = 0)
      {
         super();
         if(param2 == null)
         {
            this.moduleTypeIds = new Vector.<int>();
         }
         else
         {
            this.moduleTypeIds = param2;
         }
         this.uid = param1;
         this.max = param3;
      }
      
      public static function get instance() : ShipUpgradeModuleUpdateCommand
      {
         return _instance || (_instance = new ShipUpgradeModuleUpdateCommand());
      }
      
      public function getLibcomModuleId() : int
      {
         return 32478;
      }
      
      public function getLibcomModuleEstimatedLength() : int
      {
         return 12;
      }
      
      public function read(param1:IDataInput) : void
      {
         var _loc4_:* = 0;
         var _loc2_:int = 0;
         var _loc3_:int = 0;
         while(this.moduleTypeIds.length > 0)
         {
            this.moduleTypeIds.pop();
         }
         _loc2_ = 0;
         _loc3_ = int(param1.readByte());
         while(_loc2_ < _loc3_)
         {
            _loc4_ = int(param1.readInt());
            _loc4_ = _loc4_ >>> 1 | _loc4_ << 31;
            this.moduleTypeIds.push(_loc4_);
            _loc2_++;
         }
         this.uid = param1.readInt();
         this.uid = this.uid >>> 3 | this.uid << 29;
         this.max = param1.readInt();
         this.max = this.max >>> 13 | this.max << 19;
      }
      
      public function write(param1:IDataOutput) : void
      {
         var _loc2_:int = 0;
         param1.writeShort(32478);
         param1.writeByte(this.moduleTypeIds.length);
         for each(_loc2_ in this.moduleTypeIds)
         {
            param1.writeInt(_loc2_ << 1 | _loc2_ >>> 31);
         }
         param1.writeInt(this.uid << 3 | this.uid >>> 29);
         param1.writeInt(this.max << 13 | this.max >>> 19);
      }
   }
}

