package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import net.bigpoint.com.module.IModule;
   
   public class SpaceBallUpdateSpeedCommand implements IModule
   {
      private static var _instance:SpaceBallUpdateSpeedCommand;
      
      public var factionId:int = 0;
      
      public var speed:int = 0;
      
      public function SpaceBallUpdateSpeedCommand(param1:int = 0, param2:int = 0)
      {
         super();
         this.factionId = param1;
         this.speed = param2;
      }
      
      public static function get instance() : SpaceBallUpdateSpeedCommand
      {
         return _instance || (_instance = new SpaceBallUpdateSpeedCommand());
      }
      
      public function getLibcomModuleId() : int
      {
         return 192;
      }
      
      public function getLibcomModuleEstimatedLength() : int
      {
         return 8;
      }
      
      public function read(param1:IDataInput) : void
      {
         this.factionId = param1.readInt();
         this.factionId = this.factionId >>> 12 | this.factionId << 20;
         this.speed = param1.readInt();
         this.speed = this.speed << 1 | this.speed >>> 31;
      }
      
      public function write(param1:IDataOutput) : void
      {
         param1.writeShort(192);
         param1.writeInt(this.factionId << 12 | this.factionId >>> 20);
         param1.writeInt(this.speed >>> 1 | this.speed << 31);
      }
   }
}

