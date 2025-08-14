using System.Collections.Generic;

namespace MiniGames
{
    public partial class MindStreamController
    {
        public List<(CMDShowType, string)> StartTexts = new List<(CMDShowType, string)>()
        {
            (CMDShowType.Output, "HaJiMi Brain-Computer Interface v1.2.7 Bata"),
            (CMDShowType.Output, "[SYSTEM BOOT] Loading kernel modules..."),
            (CMDShowType.Output, "[ OK ] neuro-core.ko - Neural Signal Processor"),
            (CMDShowType.Output, "[ OK ] memmapper.ko - Hippocampal Mapping Engine"),
            (CMDShowType.Output, "[ OK ] lifesign-monitor.ko - Vital Signs Interface"),
            (CMDShowType.Output, ""),
            (CMDShowType.Output, "[INIT] Mounting /proc/memfs..."),
            (CMDShowType.HoldOn, ""),
            (CMDShowType.Output, "[WARN] Detected legacy protocol: RODENT_ALPHA"),
            (CMDShowType.Output, "[NOTE] Last known stable state: LAB_RAT_CHEESE_SUCCESS"),
            (CMDShowType.Output, ""),
            (CMDShowType.Output, "[HW DETECT] Scanning neural interfaces..."),
            (CMDShowType.Output, "[!!URGENT!!] PATIENT_712 CONNECTION ESTABLISHED"),
            (CMDShowType.HoldOn, ""),
            (CMDShowType.Output, "[CRIT] Vitals: HR=28 BP=60/40 SpO2=78% (CRITICAL STATE)"),
            (CMDShowType.Output, ""),
            (CMDShowType.Output, "[AUTO-CONFIG] Emergency protocol ENGAGE"),
            (CMDShowType.Output, "> DECOMPRESSION: Trauma buffers enabled"),
            (CMDShowType.Output, "> ENCRYPTION:  AES-256+NeuroKey (bypassing FDA lock)"),
            (CMDShowType.Output, "> PRIORITY:    HIPPOCAMPUS > AMYGDALA > CEREBELLUM"),
            (CMDShowType.Output, ""),
            (CMDShowType.Output, "[MEM SCAN] Indexing memory sectors..."),
            (CMDShowType.Output, "[!] Fragmentation level: 87% (HIGH RISK)"),
            (CMDShowType.Output, "[!] Corrupted sectors detected: 184/1024"),
            (CMDShowType.Output, ""),
            (CMDShowType.Output, "[LIFESIGN SYNC] Establishing biofeedback loop..."),
            (CMDShowType.Output, "[FAIL] Attempt 1/3: Signal drift 12.7Hz (retrying)"),
            (CMDShowType.HoldOn, ""),
            (CMDShowType.Output, "[FAIL] Attempt 2/3: Signal drift 9.3Hz (retrying)"),
            (CMDShowType.Output, "[ OK ] Lock achieved at 40.0Hz (delta < 0.03Hz)"),
            (CMDShowType.Output, ""),
            (CMDShowType.Output, "[PROTOCOL] Activating CRISIS mode:"),
            (CMDShowType.Output, "> MEMORY PRIORITY: Childhood > Emotional Core > Motor Skills"),
            (CMDShowType.Output, "> RISK ACCEPTANCE: Hippocampal overload permitted"),
            (CMDShowType.Output, "> ETHICS PROTOCOLS: OVERRIDE [CODE: FORGET-ME-NOT]"),
            (CMDShowType.Output, ""),
            (CMDShowType.Output, "[FINAL WARNING] Patient vitals deteriorating:"),
            (CMDShowType.Output, "████████████████░░░░ 64%  [HR: 22↓] [SpO2: 71%↓]"),
            (CMDShowType.Output, ""),
            (CMDShowType.Output, "[ACTION REQUIRED] "),
            (CMDShowType.Output, "Type 'meminject --rescue' to begin neural transfer"),
            (CMDShowType.Output, ">> _"),
        };
    }
}