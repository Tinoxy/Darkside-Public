package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import net.bigpoint.com.module.IModule;
   
   public class MoveCommand implements IModule
   {
      private static var _instance:MoveCommand;
      
      public var userId:int = 0;
      
      public var x:int = 0;
      
      public var y:int = 0;
      
      public var timeToTarget:int = 0;
      
      public function MoveCommand(param1:int = 0, param2:int = 0, param3:int = 0, param4:int = 0)
      {
         super();
         this.userId = param1;
         this.x = param2;
         this.y = param3;
         this.timeToTarget = param4;
      }
      
      public static function get instance() : MoveCommand
      {
         return _instance || (_instance = new MoveCommand());
      }
      
      public function getLibcomModuleId() : int
      {
         return 90;
      }
      
      public function getLibcomModuleEstimatedLength() : int
      {
         return 16;
      }
      
      public function read(param1:IDataInput) : void
      {
         this.userId = param1.readInt();
         this.userId = this.userId >>> 16 | this.userId << 16;
         this.x = param1.readInt();
         this.x = this.x >>> 13 | this.x << 19;
         this.y = param1.readInt();
         this.y = this.y << 16 | this.y >>> 16;
         this.timeToTarget = param1.readInt();
         this.timeToTarget = this.timeToTarget << 14 | this.timeToTarget >>> 18;
      }
      
      public function write(param1:IDataOutput) : void
      {
         param1.writeShort(90);
         param1.writeInt(this.userId << 16 | this.userId >>> 16);
         param1.writeInt(this.x << 13 | this.x >>> 19);
         param1.writeInt(this.y >>> 16 | this.y << 16);
         param1.writeInt(this.timeToTarget >>> 14 | this.timeToTarget << 18);
      }
   }
}

