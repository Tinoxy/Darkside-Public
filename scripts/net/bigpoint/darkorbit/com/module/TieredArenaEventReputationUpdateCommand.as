package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import net.bigpoint.com.module.IModule;
   
   public class TieredArenaEventReputationUpdateCommand implements IModule
   {
      private static var _instance:TieredArenaEventReputationUpdateCommand;
      
      public var reputation:int = 0;
      
      public var uid:int = 0;
      
      public function TieredArenaEventReputationUpdateCommand(param1:int = 0, param2:int = 0)
      {
         super();
         this.reputation = param2;
         this.uid = param1;
      }
      
      public static function get instance() : TieredArenaEventReputationUpdateCommand
      {
         return _instance || (_instance = new TieredArenaEventReputationUpdateCommand());
      }
      
      public function getLibcomModuleId() : int
      {
         return 23473;
      }
      
      public function getLibcomModuleEstimatedLength() : int
      {
         return 8;
      }
      
      public function read(param1:IDataInput) : void
      {
         this.reputation = param1.readInt();
         this.reputation = this.reputation >>> 13 | this.reputation << 19;
         this.uid = param1.readInt();
         this.uid = this.uid >>> 3 | this.uid << 29;
      }
      
      public function write(param1:IDataOutput) : void
      {
         param1.writeShort(23473);
         param1.writeInt(this.reputation << 13 | this.reputation >>> 19);
         param1.writeInt(this.uid << 3 | this.uid >>> 29);
      }
   }
}

