package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import net.bigpoint.com.module.IModule;
   
   public class WorldBossRewardsTierUpdateCommand implements IModule
   {
      private static var _instance:WorldBossRewardsTierUpdateCommand;
      
      public var currentTier:int = 0;
      
      public function WorldBossRewardsTierUpdateCommand(param1:int = 0)
      {
         super();
         this.currentTier = param1;
      }
      
      public static function get instance() : WorldBossRewardsTierUpdateCommand
      {
         return _instance || (_instance = new WorldBossRewardsTierUpdateCommand());
      }
      
      public function getLibcomModuleId() : int
      {
         return -29973;
      }
      
      public function getLibcomModuleEstimatedLength() : int
      {
         return 4;
      }
      
      public function read(param1:IDataInput) : void
      {
         this.currentTier = param1.readInt();
         this.currentTier = this.currentTier << 16 | this.currentTier >>> 16;
      }
      
      public function write(param1:IDataOutput) : void
      {
         param1.writeShort(-29973);
         param1.writeInt(this.currentTier >>> 16 | this.currentTier << 16);
      }
   }
}

