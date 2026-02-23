package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import net.bigpoint.com.module.IModule;
   
   public class AgatusEventBossInfoCommand implements IModule
   {
      private static var _instance:AgatusEventBossInfoCommand;
      
      public var bossCount:int = 0;
      
      public var leftToKill:int = 0;
      
      public function AgatusEventBossInfoCommand(param1:int = 0, param2:int = 0)
      {
         super();
         this.bossCount = param2;
         this.leftToKill = param1;
      }
      
      public static function get instance() : AgatusEventBossInfoCommand
      {
         return _instance || (_instance = new AgatusEventBossInfoCommand());
      }
      
      public function getLibcomModuleId() : int
      {
         return -12911;
      }
      
      public function getLibcomModuleEstimatedLength() : int
      {
         return 8;
      }
      
      public function read(param1:IDataInput) : void
      {
         this.bossCount = param1.readInt();
         this.bossCount = this.bossCount << 13 | this.bossCount >>> 19;
         this.leftToKill = param1.readInt();
         this.leftToKill = this.leftToKill >>> 13 | this.leftToKill << 19;
      }
      
      public function write(param1:IDataOutput) : void
      {
         param1.writeShort(-12911);
         param1.writeInt(this.bossCount >>> 13 | this.bossCount << 19);
         param1.writeInt(this.leftToKill << 13 | this.leftToKill >>> 19);
      }
   }
}

