package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import net.bigpoint.com.module.IModule;
   
   public class MemberLeaveCommand implements IModule
   {
      private static var _instance:MemberLeaveCommand;
      
      public static const LEAVEREASON_NONE:int = 0;
      
      public static const LEAVEREASON_LEAVE:int = 1;
      
      public static const LEAVEREASON_KICK:int = 2;
      
      public static const LEAVEREASON__MAX:int = 3;
      
      public var playerId:int = 0;
      
      public var reason:int = 0;
      
      public function MemberLeaveCommand(param1:int = 0, param2:int = 0)
      {
         super();
         this.playerId = param1;
         this.reason = param2;
      }
      
      public static function get instance() : MemberLeaveCommand
      {
         return _instance || (_instance = new MemberLeaveCommand());
      }
      
      public function getLibcomModuleId() : int
      {
         return -576;
      }
      
      public function getLibcomModuleEstimatedLength() : int
      {
         return 4;
      }
      
      public function read(param1:IDataInput) : void
      {
         this.playerId = param1.readInt();
         this.playerId = this.playerId >>> 3 | this.playerId << 29;
         this.reason = param1.readShort();
      }
      
      public function write(param1:IDataOutput) : void
      {
         param1.writeShort(-576);
         param1.writeInt(this.playerId << 3 | this.playerId >>> 29);
         param1.writeShort(this.reason);
      }
   }
}

