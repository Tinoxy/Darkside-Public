package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import net.bigpoint.com.module.IModule;
   
   public class AttackRocketRequest implements IModule
   {
      private static var _instance:AttackRocketRequest;
      
      public var targetId:int = 0;
      
      public var targetX:int = 0;
      
      public var targetY:int = 0;
      
      public function AttackRocketRequest(param1:int = 0, param2:int = 0, param3:int = 0)
      {
         super();
         this.targetId = param1;
         this.targetX = param2;
         this.targetY = param3;
      }
      
      public static function get instance() : AttackRocketRequest
      {
         return _instance || (_instance = new AttackRocketRequest());
      }
      
      public function getLibcomModuleId() : int
      {
         return 89;
      }
      
      public function getLibcomModuleEstimatedLength() : int
      {
         return 12;
      }
      
      public function read(param1:IDataInput) : void
      {
         this.targetId = param1.readInt();
         this.targetId = this.targetId >>> 2 | this.targetId << 30;
         this.targetX = param1.readInt();
         this.targetX = this.targetX >>> 12 | this.targetX << 20;
         this.targetY = param1.readInt();
         this.targetY = this.targetY << 14 | this.targetY >>> 18;
      }
      
      public function write(param1:IDataOutput) : void
      {
         param1.writeShort(89);
         param1.writeInt(this.targetId << 2 | this.targetId >>> 30);
         param1.writeInt(this.targetX << 12 | this.targetX >>> 20);
         param1.writeInt(this.targetY >>> 14 | this.targetY << 18);
      }
   }
}

