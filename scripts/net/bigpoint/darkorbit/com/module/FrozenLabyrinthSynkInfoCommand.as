package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import net.bigpoint.com.module.IModule;
   
   public class FrozenLabyrinthSynkInfoCommand implements IModule
   {
      private static var _instance:FrozenLabyrinthSynkInfoCommand;
      
      public var isGateActive:Boolean = false;
      
      public var mapName:String = "";
      
      public var respawnInSeconds:Number = 0;
      
      public var zone:int = 0;
      
      public function FrozenLabyrinthSynkInfoCommand(param1:String = "", param2:int = 0, param3:Number = 0, param4:Boolean = false)
      {
         super();
         this.isGateActive = param4;
         this.mapName = param1;
         this.respawnInSeconds = param3;
         this.zone = param2;
      }
      
      public static function get instance() : FrozenLabyrinthSynkInfoCommand
      {
         return _instance || (_instance = new FrozenLabyrinthSynkInfoCommand());
      }
      
      public function getLibcomModuleId() : int
      {
         return 18867;
      }
      
      public function getLibcomModuleEstimatedLength() : int
      {
         return 15;
      }
      
      public function read(param1:IDataInput) : void
      {
         this.isGateActive = param1.readBoolean();
         this.mapName = param1.readUTF();
         this.respawnInSeconds = param1.readDouble();
         this.zone = param1.readInt();
         this.zone = this.zone << 9 | this.zone >>> 23;
      }
      
      public function write(param1:IDataOutput) : void
      {
         param1.writeShort(18867);
         param1.writeBoolean(this.isGateActive);
         param1.writeUTF(this.mapName);
         param1.writeDouble(this.respawnInSeconds);
         param1.writeInt(this.zone >>> 9 | this.zone << 23);
      }
   }
}

