package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import net.bigpoint.com.module.IModule;
   
   public class KillUiFullMessageCommand implements IModule
   {
      private static var _instance:KillUiFullMessageCommand;
      
      public var npcId:int = 0;
      
      public var totalAmountOfNpcOnMap:int = 0;
      
      public var mapId:int = 0;
      
      public function KillUiFullMessageCommand(param1:int = 0, param2:int = 0, param3:int = 0)
      {
         super();
         this.npcId = param2;
         this.totalAmountOfNpcOnMap = param3;
         this.mapId = param1;
      }
      
      public static function get instance() : KillUiFullMessageCommand
      {
         return _instance || (_instance = new KillUiFullMessageCommand());
      }
      
      public function getLibcomModuleId() : int
      {
         return -6940;
      }
      
      public function getLibcomModuleEstimatedLength() : int
      {
         return 12;
      }
      
      public function read(param1:IDataInput) : void
      {
         this.npcId = param1.readInt();
         this.npcId = this.npcId >>> 6 | this.npcId << 26;
         this.totalAmountOfNpcOnMap = param1.readInt();
         this.totalAmountOfNpcOnMap = this.totalAmountOfNpcOnMap << 10 | this.totalAmountOfNpcOnMap >>> 22;
         this.mapId = param1.readInt();
         this.mapId = this.mapId >>> 9 | this.mapId << 23;
      }
      
      public function write(param1:IDataOutput) : void
      {
         param1.writeShort(-6940);
         param1.writeInt(this.npcId << 6 | this.npcId >>> 26);
         param1.writeInt(this.totalAmountOfNpcOnMap >>> 10 | this.totalAmountOfNpcOnMap << 22);
         param1.writeInt(this.mapId << 9 | this.mapId >>> 23);
      }
   }
}

