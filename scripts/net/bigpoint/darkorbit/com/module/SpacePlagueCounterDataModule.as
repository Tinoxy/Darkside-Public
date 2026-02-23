package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import net.bigpoint.com.module.IModule;
   
   public class SpacePlagueCounterDataModule implements IModule
   {
      private static var _instance:SpacePlagueCounterDataModule;
      
      public var currentValue:int = 0;
      
      public var maxValue:int = 0;
      
      public var active:Boolean = false;
      
      public function SpacePlagueCounterDataModule(param1:int = 0, param2:int = 0, param3:Boolean = false)
      {
         super();
         this.currentValue = param2;
         this.maxValue = param1;
         this.active = param3;
      }
      
      public static function get instance() : SpacePlagueCounterDataModule
      {
         return _instance || (_instance = new SpacePlagueCounterDataModule());
      }
      
      public function getLibcomModuleId() : int
      {
         return -9107;
      }
      
      public function getLibcomModuleEstimatedLength() : int
      {
         return 9;
      }
      
      public function read(param1:IDataInput) : void
      {
         this.currentValue = param1.readInt();
         this.currentValue = this.currentValue << 15 | this.currentValue >>> 17;
         this.maxValue = param1.readInt();
         this.maxValue = this.maxValue >>> 13 | this.maxValue << 19;
         this.active = param1.readBoolean();
      }
      
      public function write(param1:IDataOutput) : void
      {
         param1.writeShort(-9107);
         param1.writeInt(this.currentValue >>> 15 | this.currentValue << 17);
         param1.writeInt(this.maxValue << 13 | this.maxValue >>> 19);
         param1.writeBoolean(this.active);
      }
   }
}

