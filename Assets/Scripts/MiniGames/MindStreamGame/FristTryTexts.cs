using System.Collections.Generic;

namespace MiniGames
{
    public partial class MindStreamController
    {
        // CMD类型，CMD中的Base语句，花费的时间，最后增加的进度
        public List<(CMDShowType, string, float, float)> fristTryTexts = new List<(CMDShowType, string, float, float)>()
        {
            (CMDShowType.Output, "HaJiMi Brain-Computer Interface v1.2.7a Bata", 0.1f, 0),

            (CMDShowType.Output, "[SYSTEM] Loading neural processors...", 0.1f, 0),
            (CMDShowType.HoldOn, "", 1.5f, 0),
            (CMDShowType.HoldOn, "", 0.8f, 0),
            (CMDShowType.Output, "[OK] Cortex mapper initialized",0.2f, 0),
            (CMDShowType.HoldOn, "", 0.7f, 0),
            (CMDShowType.Output, "[OK] Bio-signal interface ready", 0.2f, 0),

            // ===== 患者连接 ===== (耗时: 3.5s)
            (CMDShowType.Output, "[CRITICAL] PATIENT_712 CONNECTED", 0.1f, 0),
            (CMDShowType.Output, "Vitals: HR=28↓ BP=60/40↓ SpO2=78%↓",0.1f, 0),
            (CMDShowType.Output, "[WARN] Legacy protocol detected: RODENT_ALPHA", 0.2f, 0),
            (CMDShowType.HoldOn, "", 1f, 0),
            (CMDShowType.Output, "[WARN] Legacy protocol detected: RODENT_ALPHA", 0.2f, 0),
            (CMDShowType.HoldOn, "", 1f, 0),
            (CMDShowType.HoldOn, "", 0.8f, 0),
            (CMDShowType.Output, "[STATUS] Last stable state: LAB_RAT_CHEESE_SUCCESS", 0.2f, 0),
            (CMDShowType.Dialog, "1%，2%......", 1f, 0),
            
            // ===== 玩家操作1：激活急救协议 ===== (进度: +12%)
            (CMDShowType.Output, "[ACTION] Activate emergency protocol", 0, 0),
            (CMDShowType.Dialog, "10%......", 1f, 0),
            
            // ===== 记忆扫描 ===== (耗时: 4.0s)
            (CMDShowType.HoldOn, "[SCAN] Indexing memory sectors...", 1.5f, 0),
            (CMDShowType.Output, "[ALERT] Fragmentation: 87% (HIGH RISK)", 0, 0),
            (CMDShowType.Output, "[ALERT] Corrupted sectors: 184/1024", 0, 0),
            (CMDShowType.Output, "[STATUS] Scanning emotional core...",0, 0),
            (CMDShowType.HoldOn, "[STATUS] Scanning emotional core...", 2.5f, 0),

            // ===== 玩家操作2：选择记忆优先级 ===== (进度: +10%)
            (CMDShowType.Output, "[ACTION] Set memory priority:", 0.1f, 0),
            (CMDShowType.Output, "Options: [childhood] [emotion] [skills]", 0.1f, 0),
            (CMDShowType.Input, ">> activate --code=forget-me-not", 0, 12),
            (CMDShowType.Input, ">> priority --set=", 0, 10),
            (CMDShowType.Dialog, "30%......", 1f, 0),
            // ===== 生命体征同步 ===== (耗时: 5.5s)
            (CMDShowType.Output, "[SYNC] Establishing biofeedback...", 0, 0),
            (CMDShowType.HoldOn, "[FAIL] Signal drift 12.7Hz (retrying)", 1.6f, 0),
            (CMDShowType.Output, "[FAIL] Signal drift 12.7Hz (retrying)", 0.2f, 0),
            (CMDShowType.HoldOn, "[FAIL] Signal drift 9.3Hz (retrying)", 1.5f, 0),
            (CMDShowType.Output, "[FAIL] Signal drift 9.3Hz (retrying)", 0.2f, 0),
            (CMDShowType.HoldOn, "[OK] Lock at 40.0Hz ±0.03Hz", 1.7f, 0),
            (CMDShowType.Output, "[OK] Lock at 40.0Hz ±0.03Hz", 0.2f, 0),

            // ===== 玩家操作3：开始记忆传输 ===== (进度: +15%)
            (CMDShowType.Output, "[WARNING] Patient vitals deteriorating:", 0, 0),
            (CMDShowType.Output, "███████████████░░░░ 58%  HR:22↓ SpO2:71↓", 0, 0),
            (CMDShowType.Output, "[ACTION] Initiate neural transfer", 0, 0),
            (CMDShowType.Input, ">> meminject --rescue", 0, 15),
            (CMDShowType.Dialog, "40%......", 1f, 0),
            // ===== 记忆传输 ===== (耗时: 12.0s)
            (CMDShowType.Output, "[TRANSFER] Encoding episodic memories...", 0.2f, 0),
            (CMDShowType.HoldOn, "[TRANSFER] Encoding episodic memories...", 2.5f, 0),
            (CMDShowType.Output, "[MEMORY] First day at school (age 6)", 0, 0),
            (CMDShowType.Output, "[TRANSFER] Compressing emotional patterns...", 0.2f, 0),
            (CMDShowType.HoldOn, "[TRANSFER] Compressing emotional patterns...", 3f, 0),
            (CMDShowType.Output, "[MEMORY] Summer beach (age 12)", 0, 0),

            // ===== 玩家操作4：解决过载问题 ===== (进度: +14%)
            (CMDShowType.Output, "[CRITICAL] NEURAL OVERLOAD DETECTED!", 0, 0),
            (CMDShowType.Output, "[ACTION] Stabilize frequency (level 1-9)", 0, 0),
            (CMDShowType.Input, ">> neuroctl dampen -lvl=", 0, 14),

            // ===== 最终传输 ===== (耗时: 8.0s)
            (CMDShowType.Output, "[TRANSFER] Finalizing identity matrix...", 0.2f, 0),
            (CMDShowType.HoldOn, "[TRANSFER] Finalizing identity matrix...",3f, 0),
            (CMDShowType.Output, "[MEMORY] High school graduation", 0, 0),
            (CMDShowType.Dialog, "50%......", 1f, 0),
            // ===== 玩家操作5：确认保存 ===== (进度: +15%)
            (CMDShowType.Output, "[WARNING] Critical memory fragments found", 0, 0),
            (CMDShowType.Output, "[ACTION] Force preservation? (Y/N)", 0, 0),
            (CMDShowType.Input, ">> commit --force", 0, 15),

            // ===== 传输完成 ===== (耗时: 5.0s)
            (CMDShowType.Output, "[FINAL] Validating neural pathways...", 0.2f, 0),
            (CMDShowType.HoldOn, "[FINAL] Validating neural pathways...", 3.0f, 0),
            (CMDShowType.Output, "[SUCCESS] Memory preservation complete", 0, 0),
            (CMDShowType.Output, "[STATUS] Digital consciousness stabilized", 0, 0),
            (CMDShowType.Output, "[SAFE] Vitals normalized: HR 68↑ SpO2 98↑", 0, 0),
            (CMDShowType.Dialog, "我闭上眼，一向相信科学的我也开始了可哀的祈祷......", 2f, 0),
        };
    }
}