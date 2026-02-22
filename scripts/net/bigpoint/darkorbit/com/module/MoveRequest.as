package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import net.bigpoint.com.module.IModule;
   
   public class MoveRequest implements IModule
   {
      private static var _instance:MoveRequest;
      
      public var positionX:int = 0;
      
      public var targetY:int = 0;
      
      public var targetX:int = 0;
      
      public var positionY:int = 0;
      
      public function MoveRequest(param1:int = 0, param2:int = 0, param3:int = 0, param4:int = 0)
      {
         super();
         this.positionX = param1;
         this.targetY = param2;
         this.targetX = param3;
         this.positionY = param4;
      }
      
      public static function get instance() : MoveRequest
      {
         return _instance || (_instance = new MoveRequest());
      }
      
      public function getLibcomModuleId() : int
      {
         return 82;
      }
      
      public function getLibcomModuleEstimatedLength() : int
      {
         return 16;
      }
      
      public function read(param1:IDataInput) : void
      {
         this.positionX = param1.readInt();
         this.positionX = this.positionX >>> 11 | this.positionX << 21;
         this.targetY = param1.readInt();
         this.targetY = this.targetY << 15 | this.targetY >>> 17;
         this.targetX = param1.readInt();
         this.targetX = this.targetX >>> 13 | this.targetX << 19;
         this.positionY = param1.readInt();
         this.positionY = this.positionY >>> 10 | this.positionY << 22;
      }
      
      public function write(param1:IDataOutput) : void
      {
         param1.writeShort(82);
         param1.writeInt(this.positionX << 11 | this.positionX >>> 21);
         param1.writeInt(this.targetY >>> 15 | this.targetY << 17);
         param1.writeInt(this.targetX << 13 | this.targetX >>> 19);
         param1.writeInt(this.positionY << 10 | this.positionY >>> 22);
      }
   }
}

