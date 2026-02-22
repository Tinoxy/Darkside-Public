package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import net.bigpoint.com.module.IModule;
   
   public class PlayVisualEffectCommand implements IModule
   {
      private static var _instance:PlayVisualEffectCommand;
      
      public static const VISUALEFFECTTYPE_CPU_PLAGUE_CURE_WAS_USED:int = 0;
      
      public static const VISUALEFFECTTYPE_PLAGUE_IMPACT:int = 1;
      
      public static const VISUALEFFECTTYPE_LONG_RANGE_LASER_ATTACK_PLAGUE:int = 2;
      
      public static const VISUALEFFECTTYPE_NORMAL_LASER_ATTACK_PLAGUE:int = 3;
      
      public static const VISUALEFFECTTYPE_LEECH_ATTACK_PLAGUE:int = 4;
      
      public static const VISUALEFFECTTYPE_SHIELD_LEECH:int = 5;
      
      public static const VISUALEFFECTTYPE_CANNON_SKILL_THERMAL:int = 6;
      
      public static const VISUALEFFECTTYPE_CANNON_SKILL_ELECTRO:int = 7;
      
      public static const VISUALEFFECTTYPE_SOLACE:int = 8;
      
      public static const VISUALEFFECTTYPE_CHSP_SHOT:int = 9;
      
      public static const VISUALEFFECTTYPE_CHSP_OUT_BREAK:int = 10;
      
      public static const VISUALEFFECTTYPE_CHSP_HIT:int = 11;
      
      public static const VISUALEFFECTTYPE_CHSP_W_SHOT:int = 12;
      
      public static const VISUALEFFECTTYPE_CHSP_W_OUT_BREAK:int = 13;
      
      public static const VISUALEFFECTTYPE_CHSP_W_HIT:int = 14;
      
      public static const VISUALEFFECTTYPE__MAX:int = 15;
      
      public var visualEffect:String = "";
      
      public var activatorId:int = 0;
      
      public var targetIds:Vector.<int>;
      
      public function PlayVisualEffectCommand(param1:String = "", param2:int = 0, param3:Vector.<int> = null)
      {
         super();
         this.visualEffect = param1;
         this.activatorId = param2;
         if(param3 == null)
         {
            this.targetIds = new Vector.<int>();
         }
         else
         {
            this.targetIds = param3;
         }
      }
      
      public static function get instance() : PlayVisualEffectCommand
      {
         return _instance || (_instance = new PlayVisualEffectCommand());
      }
      
      public function getLibcomModuleId() : int
      {
         return 10116;
      }
      
      public function getLibcomModuleEstimatedLength() : int
      {
         return 10;
      }
      
      public function read(param1:IDataInput) : void
      {
         var _loc4_:* = 0;
         this.visualEffect = param1.readUTF();
         this.activatorId = param1.readInt();
         this.activatorId = this.activatorId >>> 12 | this.activatorId << 20;
         var _loc2_:int = 0;
         var _loc3_:int = 0;
         while(this.targetIds.length > 0)
         {
            this.targetIds.pop();
         }
         _loc2_ = 0;
         _loc3_ = int(param1.readByte());
         while(_loc2_ < _loc3_)
         {
            _loc4_ = int(param1.readInt());
            _loc4_ = _loc4_ << 9 | _loc4_ >>> 23;
            this.targetIds.push(_loc4_);
            _loc2_++;
         }
      }
      
      public function write(param1:IDataOutput) : void
      {
         var _loc2_:int = 0;
         param1.writeShort(10116);
         param1.writeUTF(this.visualEffect);
         param1.writeInt(this.activatorId << 12 | this.activatorId >>> 20);
         param1.writeByte(this.targetIds.length);
         for each(_loc2_ in this.targetIds)
         {
            param1.writeInt(_loc2_ >>> 9 | _loc2_ << 23);
         }
      }
   }
}

