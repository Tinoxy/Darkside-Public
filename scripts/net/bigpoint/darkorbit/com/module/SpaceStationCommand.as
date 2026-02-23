package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import net.bigpoint.com.module.IModule;
   
   public class SpaceStationCommand implements IModule
   {
      private static var _instance:SpaceStationCommand;
      
      public var peaceRadius:int = 0;
      
      public var posY:int = 0;
      
      public var type:int = 0;
      
      public var id:int = 0;
      
      public var posX:int = 0;
      
      public function SpaceStationCommand(param1:int = 0, param2:int = 0, param3:int = 0, param4:int = 0, param5:int = 0)
      {
         super();
         this.peaceRadius = param3;
         this.posY = param5;
         this.type = param2;
         this.id = param1;
         this.posX = param4;
      }
      
      public static function get instance() : SpaceStationCommand
      {
         return _instance || (_instance = new SpaceStationCommand());
      }
      
      public function getLibcomModuleId() : int
      {
         return -15922;
      }
      
      public function getLibcomModuleEstimatedLength() : int
      {
         return 20;
      }
      
      public function read(param1:IDataInput) : void
      {
         this.peaceRadius = param1.readInt();
         this.peaceRadius = this.peaceRadius >>> 12 | this.peaceRadius << 20;
         this.posY = param1.readInt();
         this.posY = this.posY << 15 | this.posY >>> 17;
         this.type = param1.readInt();
         this.type = this.type << 13 | this.type >>> 19;
         this.id = param1.readInt();
         this.id = this.id << 15 | this.id >>> 17;
         this.posX = param1.readInt();
         this.posX = this.posX << 12 | this.posX >>> 20;
      }
      
      public function write(param1:IDataOutput) : void
      {
         param1.writeShort(-15922);
         param1.writeInt(this.peaceRadius << 12 | this.peaceRadius >>> 20);
         param1.writeInt(this.posY >>> 15 | this.posY << 17);
         param1.writeInt(this.type >>> 13 | this.type << 19);
         param1.writeInt(this.id >>> 15 | this.id << 17);
         param1.writeInt(this.posX >>> 12 | this.posX << 20);
      }
   }
}

