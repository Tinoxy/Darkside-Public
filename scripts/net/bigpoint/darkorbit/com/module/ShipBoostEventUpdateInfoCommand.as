package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import net.bigpoint.com.module.IModule;
   
   public class ShipBoostEventUpdateInfoCommand implements IModule
   {
      private static var _instance:ShipBoostEventUpdateInfoCommand;
      
      public var currentSlot:ShipBoostEventModule;
      
      public var timeToWindowChange:Number = 0;
      
      public var isBoostActive:Boolean = false;
      
      public var boostInfo:ShipBoostEventBoostInfoCommand;
      
      public function ShipBoostEventUpdateInfoCommand(param1:Number = 0, param2:Boolean = false, param3:ShipBoostEventModule = null, param4:ShipBoostEventBoostInfoCommand = null)
      {
         super();
         if(param3 == null)
         {
            this.currentSlot = new ShipBoostEventModule();
         }
         else
         {
            this.currentSlot = param3;
         }
         this.timeToWindowChange = param1;
         this.isBoostActive = param2;
         if(param4 == null)
         {
            this.boostInfo = new ShipBoostEventBoostInfoCommand();
         }
         else
         {
            this.boostInfo = param4;
         }
      }
      
      public static function get instance() : ShipBoostEventUpdateInfoCommand
      {
         return _instance || (_instance = new ShipBoostEventUpdateInfoCommand());
      }
      
      public function getLibcomModuleId() : int
      {
         return -11108;
      }
      
      public function getLibcomModuleEstimatedLength() : int
      {
         return 9;
      }
      
      public function read(param1:IDataInput) : void
      {
         this.currentSlot = ShipBoostEventModule(ModuleFactory.get().createInstance(param1.readShort()));
         if(null != this.currentSlot)
         {
            this.currentSlot.read(param1);
         }
         this.timeToWindowChange = param1.readDouble();
         this.isBoostActive = param1.readBoolean();
         this.boostInfo = ShipBoostEventBoostInfoCommand(ModuleFactory.get().createInstance(param1.readShort()));
         if(null != this.boostInfo)
         {
            this.boostInfo.read(param1);
         }
      }
      
      public function write(param1:IDataOutput) : void
      {
         param1.writeShort(-11108);
         if(null != this.currentSlot)
         {
            this.currentSlot.write(param1);
         }
         else
         {
            param1.writeShort(0);
         }
         param1.writeDouble(this.timeToWindowChange);
         param1.writeBoolean(this.isBoostActive);
         if(null != this.boostInfo)
         {
            this.boostInfo.write(param1);
         }
         else
         {
            param1.writeShort(0);
         }
      }
   }
}

