package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import net.bigpoint.com.module.IModule;
   
   public class RogueLiteSelectShipCommand implements IModule
   {
      private static var _instance:RogueLiteSelectShipCommand;
      
      public var shipList:Vector.<RogueLiteShipModule>;
      
      public var modifierList:Vector.<RogueLiteModifierModule>;
      
      public function RogueLiteSelectShipCommand(param1:Vector.<RogueLiteModifierModule> = null, param2:Vector.<RogueLiteShipModule> = null)
      {
         super();
         if(param2 == null)
         {
            this.shipList = new Vector.<RogueLiteShipModule>();
         }
         else
         {
            this.shipList = param2;
         }
         if(param1 == null)
         {
            this.modifierList = new Vector.<RogueLiteModifierModule>();
         }
         else
         {
            this.modifierList = param1;
         }
      }
      
      public static function get instance() : RogueLiteSelectShipCommand
      {
         return _instance || (_instance = new RogueLiteSelectShipCommand());
      }
      
      public function getLibcomModuleId() : int
      {
         return -24339;
      }
      
      public function getLibcomModuleEstimatedLength() : int
      {
         return 8;
      }
      
      public function read(param1:IDataInput) : void
      {
         var _loc4_:RogueLiteShipModule = null;
         var _loc5_:RogueLiteModifierModule = null;
         var _loc2_:int = 0;
         var _loc3_:int = 0;
         while(this.shipList.length > 0)
         {
            this.shipList.pop();
         }
         _loc2_ = 0;
         _loc3_ = int(param1.readByte());
         while(_loc2_ < _loc3_)
         {
            _loc4_ = RogueLiteShipModule(ModuleFactory.get().createInstance(param1.readShort()));
            _loc4_.read(param1);
            this.shipList.push(_loc4_);
            _loc2_++;
         }
         while(this.modifierList.length > 0)
         {
            this.modifierList.pop();
         }
         _loc2_ = 0;
         _loc3_ = int(param1.readByte());
         while(_loc2_ < _loc3_)
         {
            _loc5_ = RogueLiteModifierModule(ModuleFactory.get().createInstance(param1.readShort()));
            _loc5_.read(param1);
            this.modifierList.push(_loc5_);
            _loc2_++;
         }
      }
      
      public function write(param1:IDataOutput) : void
      {
         var _loc2_:RogueLiteShipModule = null;
         var _loc3_:RogueLiteModifierModule = null;
         param1.writeShort(-24339);
         param1.writeByte(this.shipList.length);
         for each(_loc2_ in this.shipList)
         {
            _loc2_.write(param1);
         }
         param1.writeByte(this.modifierList.length);
         for each(_loc3_ in this.modifierList)
         {
            _loc3_.write(param1);
         }
      }
   }
}

