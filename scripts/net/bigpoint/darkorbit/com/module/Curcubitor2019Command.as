package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import net.bigpoint.com.module.IModule;
   
   public class Curcubitor2019Command implements IModule
   {
      private static var _instance:Curcubitor2019Command;
      
      public var timeToNext:Number = 0;
      
      public var curcubitorLeftToKill:int = 0;
      
      public var bossCount:int = 0;
      
      public function Curcubitor2019Command(param1:int = 0, param2:int = 0, param3:Number = 0)
      {
         super();
         this.timeToNext = param3;
         this.curcubitorLeftToKill = param1;
         this.bossCount = param2;
      }
      
      public static function get instance() : Curcubitor2019Command
      {
         return _instance || (_instance = new Curcubitor2019Command());
      }
      
      public function getLibcomModuleId() : int
      {
         return 14232;
      }
      
      public function getLibcomModuleEstimatedLength() : int
      {
         return 16;
      }
      
      public function read(param1:IDataInput) : void
      {
         this.timeToNext = param1.readDouble();
         this.curcubitorLeftToKill = param1.readInt();
         this.curcubitorLeftToKill = this.curcubitorLeftToKill >>> 12 | this.curcubitorLeftToKill << 20;
         this.bossCount = param1.readInt();
         this.bossCount = this.bossCount >>> 9 | this.bossCount << 23;
      }
      
      public function write(param1:IDataOutput) : void
      {
         param1.writeShort(14232);
         param1.writeDouble(this.timeToNext);
         param1.writeInt(this.curcubitorLeftToKill << 12 | this.curcubitorLeftToKill >>> 20);
         param1.writeInt(this.bossCount << 9 | this.bossCount >>> 23);
      }
   }
}

