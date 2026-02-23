package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import net.bigpoint.com.module.IModule;
   
   public class ArenaSurvivorsUpdateCommand implements IModule
   {
      private static var _instance:ArenaSurvivorsUpdateCommand;
      
      public var arenaId:int = 0;
      
      public var participants:int = 0;
      
      public var survivors:int = 0;
      
      public function ArenaSurvivorsUpdateCommand(param1:int = 0, param2:int = 0, param3:int = 0)
      {
         super();
         this.arenaId = param1;
         this.participants = param3;
         this.survivors = param2;
      }
      
      public static function get instance() : ArenaSurvivorsUpdateCommand
      {
         return _instance || (_instance = new ArenaSurvivorsUpdateCommand());
      }
      
      public function getLibcomModuleId() : int
      {
         return -3212;
      }
      
      public function getLibcomModuleEstimatedLength() : int
      {
         return 12;
      }
      
      public function read(param1:IDataInput) : void
      {
         this.arenaId = param1.readInt();
         this.arenaId = this.arenaId << 11 | this.arenaId >>> 21;
         this.participants = param1.readInt();
         this.participants = this.participants >>> 5 | this.participants << 27;
         this.survivors = param1.readInt();
         this.survivors = this.survivors << 1 | this.survivors >>> 31;
      }
      
      public function write(param1:IDataOutput) : void
      {
         param1.writeShort(-3212);
         param1.writeInt(this.arenaId >>> 11 | this.arenaId << 21);
         param1.writeInt(this.participants << 5 | this.participants >>> 27);
         param1.writeInt(this.survivors >>> 1 | this.survivors << 31);
      }
   }
}

