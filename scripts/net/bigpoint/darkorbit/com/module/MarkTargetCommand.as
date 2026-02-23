package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import net.bigpoint.com.module.IModule;
   
   public class MarkTargetCommand implements IModule
   {
      private static var _instance:MarkTargetCommand;
      
      public var userId:int = 0;
      
      public var targetId:int = 0;
      
      public var positionX:int = 0;
      
      public var positionY:int = 0;
      
      public function MarkTargetCommand(param1:int = 0, param2:int = 0, param3:int = 0, param4:int = 0)
      {
         super();
         this.userId = param1;
         this.targetId = param2;
         this.positionX = param3;
         this.positionY = param4;
      }
      
      public static function get instance() : MarkTargetCommand
      {
         return _instance || (_instance = new MarkTargetCommand());
      }
      
      public function getLibcomModuleId() : int
      {
         return 281;
      }
      
      public function getLibcomModuleEstimatedLength() : int
      {
         return 16;
      }
      
      public function read(param1:IDataInput) : void
      {
         this.userId = param1.readInt();
         this.userId = this.userId << 2 | this.userId >>> 30;
         this.targetId = param1.readInt();
         this.targetId = this.targetId >>> 13 | this.targetId << 19;
         this.positionX = param1.readInt();
         this.positionX = this.positionX >>> 5 | this.positionX << 27;
         this.positionY = param1.readInt();
         this.positionY = this.positionY >>> 2 | this.positionY << 30;
      }
      
      public function write(param1:IDataOutput) : void
      {
         param1.writeShort(281);
         param1.writeInt(this.userId >>> 2 | this.userId << 30);
         param1.writeInt(this.targetId << 13 | this.targetId >>> 19);
         param1.writeInt(this.positionX << 5 | this.positionX >>> 27);
         param1.writeInt(this.positionY << 2 | this.positionY >>> 30);
      }
   }
}

