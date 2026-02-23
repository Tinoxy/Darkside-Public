package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import net.bigpoint.com.module.IModule;
   
   public class JumpInitiatedCommand implements IModule
   {
      private static var _instance:JumpInitiatedCommand;
      
      public var mapId:int = 0;
      
      public var gateId:int = 0;
      
      public function JumpInitiatedCommand(param1:int = 0, param2:int = 0)
      {
         super();
         this.mapId = param1;
         this.gateId = param2;
      }
      
      public static function get instance() : JumpInitiatedCommand
      {
         return _instance || (_instance = new JumpInitiatedCommand());
      }
      
      public function getLibcomModuleId() : int
      {
         return 19;
      }
      
      public function getLibcomModuleEstimatedLength() : int
      {
         return 8;
      }
      
      public function read(param1:IDataInput) : void
      {
         this.mapId = param1.readInt();
         this.mapId = this.mapId << 7 | this.mapId >>> 25;
         this.gateId = param1.readInt();
         this.gateId = this.gateId << 6 | this.gateId >>> 26;
      }
      
      public function write(param1:IDataOutput) : void
      {
         param1.writeShort(19);
         param1.writeInt(this.mapId >>> 7 | this.mapId << 25);
         param1.writeInt(this.gateId >>> 6 | this.gateId << 26);
      }
   }
}

