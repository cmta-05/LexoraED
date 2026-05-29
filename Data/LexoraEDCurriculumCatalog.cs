using LexoraED.Models;

namespace LexoraED.Data;

public sealed record QuizQuestionDefinition(
    QuizQuestionType QuestionType,
    string QuestionText,
    string ChoiceA,
    string ChoiceB,
    string ChoiceC,
    string ChoiceD,
    string CorrectAnswer,
    string? Explanation = null);

public sealed record CurriculumModuleDefinition(
    string Title,
    string Description,
    string Content,
    ModuleCategory Category,
    DifficultyLevel DifficultyLevel,
    int SortOrder,
    int? PrerequisiteSortOrder,
    IReadOnlyList<QuizQuestionDefinition> Questions);

public static class LexoraEDCurriculumCatalog
{
    public static IReadOnlyList<CurriculumModuleDefinition> GetAllModules()
        => BeginnerModules()
            .Concat(IntermediateModules())
            .Concat(AdvancedModules())
            .ToList();

    private static IEnumerable<CurriculumModuleDefinition> BeginnerModules()
    {
        yield return new CurriculumModuleDefinition(
            "Basic Greetings and Introductions",
            "Learn essential English greetings for everyday meetings.",
            @"## Introduction
Welcome to Basic Greetings and Introductions. In this microlesson you will study basic greetings and introductions through clear explanations, guided practice, and short examples you can use immediately in class or conversation. English becomes easier when you connect new words and patterns to situations you already know, such as meeting classmates, answering the teacher, or describing your day. Read each section carefully and say the sample sentences aloud to build confidence and pronunciation. LexoraED modules are designed as microlessons: each section builds on the previous one so you never feel lost. Take notes in your own language first if that helps, then rewrite key ideas in English. Learning a language is a process, so celebrate small wins such as remembering one new phrase or fixing one grammar mistake during a role-play.

## Learning Objectives
By the end of this lesson you will be able to recognize key words and phrases, use them in simple sentences, answer short questions about the topic, and avoid common beginner mistakes. You will also practice listening for meaning, choosing appropriate responses, and reviewing what you learned with the module quiz. You should be able to explain the topic to a partner in one or two sentences, complete a short written exercise without copying from the screen, and identify at least three expressions that are appropriate in polite conversation. Finally, you will prepare for the twelve-question assessment that checks multiple choice, true or false, identification, and situational judgment skills related to this topic.

## Main Discussion
The main focus of Basic Greetings and Introductions is basic greetings and introductions. Start with high-frequency expressions learners hear every day. Notice spelling, stress, and polite forms. When you speak, use short sentences: subject plus verb plus complement. Repeat new items in context instead of memorizing isolated lists. Pair words with gestures or pictures when possible. If you are unsure, ask a clarifying question such as Could you repeat that, please? Keep a small notebook for examples you personally need at school, work, or online study. Compare formal and informal registers when the lesson allows: some phrases fit friends, while others fit teachers or supervisors. Watch for false friends from your first language that look like English but mean something different. Record yourself for thirty seconds and listen for clarity. Discuss the topic with a study partner and correct each other kindly. When reading, underline new vocabulary and write a synonym you already know beside each word.

## Examples
Example 1: Learner A greets Learner B in the morning and asks a simple follow-up question. Example 2: Learner describes one fact using the target vocabulary in a complete sentence. Example 3: Learner listens to a short prompt and selects the best response from two options. Example 4: Learner rewrites a broken sentence into a correct and polite English line. Example 5: Two learners role-play a classroom scenario and swap roles so both speak and listen. Example 6: Learner writes three original sentences, checks them against the lesson notes, and corrects any tense or word-order errors before submitting practice work.

## Summary
You reviewed Basic Greetings and Introductions by studying introduction, objectives, discussion, and examples. Continue practicing aloud for five minutes after each study session. Complete the quiz to check understanding, then revisit any missed questions and read the explanations. Strong learners review little and often rather than cramming once a week. Return to this module when you need a refresher before presentations, interviews, or exams. Share one new sentence with a friend or family member to reinforce memory. Mark the module complete in LexoraED only after you feel comfortable using the language without reading every word from the screen.",
            ModuleCategory.Vocabulary,
            DifficultyLevel.Beginner,
            1,
            null,
            new List<QuizQuestionDefinition>
            {
                new(QuizQuestionType.MultipleChoice, "Which option best matches the main idea of Basic Greetings and Introductions?", "An unrelated science term", "A correct core concept", "A random number only", "A punctuation mark only", "B", "Choose the concept taught in the lesson."),
            new(QuizQuestionType.MultipleChoice, "When practicing Basic Greetings and Introductions, what helps most?", "Memorizing with no context", "Using words in real sentences", "Avoiding all speaking", "Skipping review", "B"),
            new(QuizQuestionType.MultipleChoice, "Which sentence is grammatically appropriate for Basic Greetings and Introductions?", "Me go store yesterday always", "She study careful every day", "They are learning step by step", "Him is go now quick", "C"),
            new(QuizQuestionType.MultipleChoice, "Select the polite response related to Basic Greetings and Introductions.", "Go away now", "Could you repeat that, please?", "I no listen", "That stupid question", "B"),
            new(QuizQuestionType.MultipleChoice, "What should you do after studying Basic Greetings and Introductions?", "Never practice again", "Practice aloud briefly", "Delete your notes", "Ignore the quiz", "B"),
            new(QuizQuestionType.MultipleChoice, "Which study habit supports Basic Greetings and Introductions?", "Cram once a month only", "Review a few minutes often", "Skip all examples", "Avoid asking questions", "B"),
            new(QuizQuestionType.TrueFalse, "True or False: Connecting Basic Greetings and Introductions to daily situations improves retention.", "True", "False", "", "", "True", "Context helps memory."),
            new(QuizQuestionType.TrueFalse, "True or False: You should never speak new words aloud when learning Basic Greetings and Introductions.", "True", "False", "", "", "False", "Speaking supports pronunciation."),
            new(QuizQuestionType.Identification, "Fill in the blank: Good ______ is important when learning Basic Greetings and Introductions.", "", "", "", "", "practice"),
            new(QuizQuestionType.Identification, "Write the missing word: Review your ______ after each lesson on Basic Greetings and Introductions.", "", "", "", "", "notes"),
            new(QuizQuestionType.Situational, "You forget a word during a Basic Greetings and Introductions activity. What is the best action?", "Stay silent forever", "Ask for clarification politely", "Leave the room angrily", "Change the subject rudely", "clarification|repeat|please"),
            new(QuizQuestionType.Situational, "A classmate needs help with Basic Greetings and Introductions. What should you do?", "Mock their mistake", "Offer a short helpful example", "Ignore them completely", "Copy their work", "help|example|support")
            });
        yield return new CurriculumModuleDefinition(
            "Self Introduction",
            "Introduce yourself clearly using name, origin, and interests.",
            @"## Introduction
Welcome to Self Introduction. In this microlesson you will study self introduction through clear explanations, guided practice, and short examples you can use immediately in class or conversation. English becomes easier when you connect new words and patterns to situations you already know, such as meeting classmates, answering the teacher, or describing your day. Read each section carefully and say the sample sentences aloud to build confidence and pronunciation. LexoraED modules are designed as microlessons: each section builds on the previous one so you never feel lost. Take notes in your own language first if that helps, then rewrite key ideas in English. Learning a language is a process, so celebrate small wins such as remembering one new phrase or fixing one grammar mistake during a role-play.

## Learning Objectives
By the end of this lesson you will be able to recognize key words and phrases, use them in simple sentences, answer short questions about the topic, and avoid common beginner mistakes. You will also practice listening for meaning, choosing appropriate responses, and reviewing what you learned with the module quiz. You should be able to explain the topic to a partner in one or two sentences, complete a short written exercise without copying from the screen, and identify at least three expressions that are appropriate in polite conversation. Finally, you will prepare for the twelve-question assessment that checks multiple choice, true or false, identification, and situational judgment skills related to this topic.

## Main Discussion
The main focus of Self Introduction is self introduction. Start with high-frequency expressions learners hear every day. Notice spelling, stress, and polite forms. When you speak, use short sentences: subject plus verb plus complement. Repeat new items in context instead of memorizing isolated lists. Pair words with gestures or pictures when possible. If you are unsure, ask a clarifying question such as Could you repeat that, please? Keep a small notebook for examples you personally need at school, work, or online study. Compare formal and informal registers when the lesson allows: some phrases fit friends, while others fit teachers or supervisors. Watch for false friends from your first language that look like English but mean something different. Record yourself for thirty seconds and listen for clarity. Discuss the topic with a study partner and correct each other kindly. When reading, underline new vocabulary and write a synonym you already know beside each word.

## Examples
Example 1: Learner A greets Learner B in the morning and asks a simple follow-up question. Example 2: Learner describes one fact using the target vocabulary in a complete sentence. Example 3: Learner listens to a short prompt and selects the best response from two options. Example 4: Learner rewrites a broken sentence into a correct and polite English line. Example 5: Two learners role-play a classroom scenario and swap roles so both speak and listen. Example 6: Learner writes three original sentences, checks them against the lesson notes, and corrects any tense or word-order errors before submitting practice work.

## Summary
You reviewed Self Introduction by studying introduction, objectives, discussion, and examples. Continue practicing aloud for five minutes after each study session. Complete the quiz to check understanding, then revisit any missed questions and read the explanations. Strong learners review little and often rather than cramming once a week. Return to this module when you need a refresher before presentations, interviews, or exams. Share one new sentence with a friend or family member to reinforce memory. Mark the module complete in LexoraED only after you feel comfortable using the language without reading every word from the screen.",
            ModuleCategory.Vocabulary,
            DifficultyLevel.Beginner,
            2,
            1,
            new List<QuizQuestionDefinition>
            {
                new(QuizQuestionType.MultipleChoice, "Which option best matches the main idea of Self Introduction?", "An unrelated science term", "A correct core concept", "A random number only", "A punctuation mark only", "B", "Choose the concept taught in the lesson."),
            new(QuizQuestionType.MultipleChoice, "When practicing Self Introduction, what helps most?", "Memorizing with no context", "Using words in real sentences", "Avoiding all speaking", "Skipping review", "B"),
            new(QuizQuestionType.MultipleChoice, "Which sentence is grammatically appropriate for Self Introduction?", "Me go store yesterday always", "She study careful every day", "They are learning step by step", "Him is go now quick", "C"),
            new(QuizQuestionType.MultipleChoice, "Select the polite response related to Self Introduction.", "Go away now", "Could you repeat that, please?", "I no listen", "That stupid question", "B"),
            new(QuizQuestionType.MultipleChoice, "What should you do after studying Self Introduction?", "Never practice again", "Practice aloud briefly", "Delete your notes", "Ignore the quiz", "B"),
            new(QuizQuestionType.MultipleChoice, "Which study habit supports Self Introduction?", "Cram once a month only", "Review a few minutes often", "Skip all examples", "Avoid asking questions", "B"),
            new(QuizQuestionType.TrueFalse, "True or False: Connecting Self Introduction to daily situations improves retention.", "True", "False", "", "", "True", "Context helps memory."),
            new(QuizQuestionType.TrueFalse, "True or False: You should never speak new words aloud when learning Self Introduction.", "True", "False", "", "", "False", "Speaking supports pronunciation."),
            new(QuizQuestionType.Identification, "Fill in the blank: Good ______ is important when learning Self Introduction.", "", "", "", "", "practice"),
            new(QuizQuestionType.Identification, "Write the missing word: Review your ______ after each lesson on Self Introduction.", "", "", "", "", "notes"),
            new(QuizQuestionType.Situational, "You forget a word during a Self Introduction activity. What is the best action?", "Stay silent forever", "Ask for clarification politely", "Leave the room angrily", "Change the subject rudely", "clarification|repeat|please"),
            new(QuizQuestionType.Situational, "A classmate needs help with Self Introduction. What should you do?", "Mock their mistake", "Offer a short helpful example", "Ignore them completely", "Copy their work", "help|example|support")
            });
        yield return new CurriculumModuleDefinition(
            "Basic Vocabulary",
            "Build a core word bank for daily classroom and home topics.",
            @"## Introduction
Welcome to Basic Vocabulary. In this microlesson you will study basic vocabulary through clear explanations, guided practice, and short examples you can use immediately in class or conversation. English becomes easier when you connect new words and patterns to situations you already know, such as meeting classmates, answering the teacher, or describing your day. Read each section carefully and say the sample sentences aloud to build confidence and pronunciation. LexoraED modules are designed as microlessons: each section builds on the previous one so you never feel lost. Take notes in your own language first if that helps, then rewrite key ideas in English. Learning a language is a process, so celebrate small wins such as remembering one new phrase or fixing one grammar mistake during a role-play.

## Learning Objectives
By the end of this lesson you will be able to recognize key words and phrases, use them in simple sentences, answer short questions about the topic, and avoid common beginner mistakes. You will also practice listening for meaning, choosing appropriate responses, and reviewing what you learned with the module quiz. You should be able to explain the topic to a partner in one or two sentences, complete a short written exercise without copying from the screen, and identify at least three expressions that are appropriate in polite conversation. Finally, you will prepare for the twelve-question assessment that checks multiple choice, true or false, identification, and situational judgment skills related to this topic.

## Main Discussion
The main focus of Basic Vocabulary is basic vocabulary. Start with high-frequency expressions learners hear every day. Notice spelling, stress, and polite forms. When you speak, use short sentences: subject plus verb plus complement. Repeat new items in context instead of memorizing isolated lists. Pair words with gestures or pictures when possible. If you are unsure, ask a clarifying question such as Could you repeat that, please? Keep a small notebook for examples you personally need at school, work, or online study. Compare formal and informal registers when the lesson allows: some phrases fit friends, while others fit teachers or supervisors. Watch for false friends from your first language that look like English but mean something different. Record yourself for thirty seconds and listen for clarity. Discuss the topic with a study partner and correct each other kindly. When reading, underline new vocabulary and write a synonym you already know beside each word.

## Examples
Example 1: Learner A greets Learner B in the morning and asks a simple follow-up question. Example 2: Learner describes one fact using the target vocabulary in a complete sentence. Example 3: Learner listens to a short prompt and selects the best response from two options. Example 4: Learner rewrites a broken sentence into a correct and polite English line. Example 5: Two learners role-play a classroom scenario and swap roles so both speak and listen. Example 6: Learner writes three original sentences, checks them against the lesson notes, and corrects any tense or word-order errors before submitting practice work.

## Summary
You reviewed Basic Vocabulary by studying introduction, objectives, discussion, and examples. Continue practicing aloud for five minutes after each study session. Complete the quiz to check understanding, then revisit any missed questions and read the explanations. Strong learners review little and often rather than cramming once a week. Return to this module when you need a refresher before presentations, interviews, or exams. Share one new sentence with a friend or family member to reinforce memory. Mark the module complete in LexoraED only after you feel comfortable using the language without reading every word from the screen.",
            ModuleCategory.Vocabulary,
            DifficultyLevel.Beginner,
            3,
            2,
            new List<QuizQuestionDefinition>
            {
                new(QuizQuestionType.MultipleChoice, "Which option best matches the main idea of Basic Vocabulary?", "An unrelated science term", "A correct core concept", "A random number only", "A punctuation mark only", "B", "Choose the concept taught in the lesson."),
            new(QuizQuestionType.MultipleChoice, "When practicing Basic Vocabulary, what helps most?", "Memorizing with no context", "Using words in real sentences", "Avoiding all speaking", "Skipping review", "B"),
            new(QuizQuestionType.MultipleChoice, "Which sentence is grammatically appropriate for Basic Vocabulary?", "Me go store yesterday always", "She study careful every day", "They are learning step by step", "Him is go now quick", "C"),
            new(QuizQuestionType.MultipleChoice, "Select the polite response related to Basic Vocabulary.", "Go away now", "Could you repeat that, please?", "I no listen", "That stupid question", "B"),
            new(QuizQuestionType.MultipleChoice, "What should you do after studying Basic Vocabulary?", "Never practice again", "Practice aloud briefly", "Delete your notes", "Ignore the quiz", "B"),
            new(QuizQuestionType.MultipleChoice, "Which study habit supports Basic Vocabulary?", "Cram once a month only", "Review a few minutes often", "Skip all examples", "Avoid asking questions", "B"),
            new(QuizQuestionType.TrueFalse, "True or False: Connecting Basic Vocabulary to daily situations improves retention.", "True", "False", "", "", "True", "Context helps memory."),
            new(QuizQuestionType.TrueFalse, "True or False: You should never speak new words aloud when learning Basic Vocabulary.", "True", "False", "", "", "False", "Speaking supports pronunciation."),
            new(QuizQuestionType.Identification, "Fill in the blank: Good ______ is important when learning Basic Vocabulary.", "", "", "", "", "practice"),
            new(QuizQuestionType.Identification, "Write the missing word: Review your ______ after each lesson on Basic Vocabulary.", "", "", "", "", "notes"),
            new(QuizQuestionType.Situational, "You forget a word during a Basic Vocabulary activity. What is the best action?", "Stay silent forever", "Ask for clarification politely", "Leave the room angrily", "Change the subject rudely", "clarification|repeat|please"),
            new(QuizQuestionType.Situational, "A classmate needs help with Basic Vocabulary. What should you do?", "Mock their mistake", "Offer a short helpful example", "Ignore them completely", "Copy their work", "help|example|support")
            });
        yield return new CurriculumModuleDefinition(
            "Numbers in English",
            "Count, compare, and use numbers in practical situations.",
            @"## Introduction
Welcome to Numbers in English. In this microlesson you will study numbers in english through clear explanations, guided practice, and short examples you can use immediately in class or conversation. English becomes easier when you connect new words and patterns to situations you already know, such as meeting classmates, answering the teacher, or describing your day. Read each section carefully and say the sample sentences aloud to build confidence and pronunciation. LexoraED modules are designed as microlessons: each section builds on the previous one so you never feel lost. Take notes in your own language first if that helps, then rewrite key ideas in English. Learning a language is a process, so celebrate small wins such as remembering one new phrase or fixing one grammar mistake during a role-play.

## Learning Objectives
By the end of this lesson you will be able to recognize key words and phrases, use them in simple sentences, answer short questions about the topic, and avoid common beginner mistakes. You will also practice listening for meaning, choosing appropriate responses, and reviewing what you learned with the module quiz. You should be able to explain the topic to a partner in one or two sentences, complete a short written exercise without copying from the screen, and identify at least three expressions that are appropriate in polite conversation. Finally, you will prepare for the twelve-question assessment that checks multiple choice, true or false, identification, and situational judgment skills related to this topic.

## Main Discussion
The main focus of Numbers in English is numbers in english. Start with high-frequency expressions learners hear every day. Notice spelling, stress, and polite forms. When you speak, use short sentences: subject plus verb plus complement. Repeat new items in context instead of memorizing isolated lists. Pair words with gestures or pictures when possible. If you are unsure, ask a clarifying question such as Could you repeat that, please? Keep a small notebook for examples you personally need at school, work, or online study. Compare formal and informal registers when the lesson allows: some phrases fit friends, while others fit teachers or supervisors. Watch for false friends from your first language that look like English but mean something different. Record yourself for thirty seconds and listen for clarity. Discuss the topic with a study partner and correct each other kindly. When reading, underline new vocabulary and write a synonym you already know beside each word.

## Examples
Example 1: Learner A greets Learner B in the morning and asks a simple follow-up question. Example 2: Learner describes one fact using the target vocabulary in a complete sentence. Example 3: Learner listens to a short prompt and selects the best response from two options. Example 4: Learner rewrites a broken sentence into a correct and polite English line. Example 5: Two learners role-play a classroom scenario and swap roles so both speak and listen. Example 6: Learner writes three original sentences, checks them against the lesson notes, and corrects any tense or word-order errors before submitting practice work.

## Summary
You reviewed Numbers in English by studying introduction, objectives, discussion, and examples. Continue practicing aloud for five minutes after each study session. Complete the quiz to check understanding, then revisit any missed questions and read the explanations. Strong learners review little and often rather than cramming once a week. Return to this module when you need a refresher before presentations, interviews, or exams. Share one new sentence with a friend or family member to reinforce memory. Mark the module complete in LexoraED only after you feel comfortable using the language without reading every word from the screen.",
            ModuleCategory.Vocabulary,
            DifficultyLevel.Beginner,
            4,
            3,
            new List<QuizQuestionDefinition>
            {
                new(QuizQuestionType.MultipleChoice, "Which option best matches the main idea of Numbers in English?", "An unrelated science term", "A correct core concept", "A random number only", "A punctuation mark only", "B", "Choose the concept taught in the lesson."),
            new(QuizQuestionType.MultipleChoice, "When practicing Numbers in English, what helps most?", "Memorizing with no context", "Using words in real sentences", "Avoiding all speaking", "Skipping review", "B"),
            new(QuizQuestionType.MultipleChoice, "Which sentence is grammatically appropriate for Numbers in English?", "Me go store yesterday always", "She study careful every day", "They are learning step by step", "Him is go now quick", "C"),
            new(QuizQuestionType.MultipleChoice, "Select the polite response related to Numbers in English.", "Go away now", "Could you repeat that, please?", "I no listen", "That stupid question", "B"),
            new(QuizQuestionType.MultipleChoice, "What should you do after studying Numbers in English?", "Never practice again", "Practice aloud briefly", "Delete your notes", "Ignore the quiz", "B"),
            new(QuizQuestionType.MultipleChoice, "Which study habit supports Numbers in English?", "Cram once a month only", "Review a few minutes often", "Skip all examples", "Avoid asking questions", "B"),
            new(QuizQuestionType.TrueFalse, "True or False: Connecting Numbers in English to daily situations improves retention.", "True", "False", "", "", "True", "Context helps memory."),
            new(QuizQuestionType.TrueFalse, "True or False: You should never speak new words aloud when learning Numbers in English.", "True", "False", "", "", "False", "Speaking supports pronunciation."),
            new(QuizQuestionType.Identification, "Fill in the blank: Good ______ is important when learning Numbers in English.", "", "", "", "", "practice"),
            new(QuizQuestionType.Identification, "Write the missing word: Review your ______ after each lesson on Numbers in English.", "", "", "", "", "notes"),
            new(QuizQuestionType.Situational, "You forget a word during a Numbers in English activity. What is the best action?", "Stay silent forever", "Ask for clarification politely", "Leave the room angrily", "Change the subject rudely", "clarification|repeat|please"),
            new(QuizQuestionType.Situational, "A classmate needs help with Numbers in English. What should you do?", "Mock their mistake", "Offer a short helpful example", "Ignore them completely", "Copy their work", "help|example|support")
            });
        yield return new CurriculumModuleDefinition(
            "Colors and Descriptions",
            "Name colors and describe objects with simple adjectives.",
            @"## Introduction
Welcome to Colors and Descriptions. In this microlesson you will study colors and descriptions through clear explanations, guided practice, and short examples you can use immediately in class or conversation. English becomes easier when you connect new words and patterns to situations you already know, such as meeting classmates, answering the teacher, or describing your day. Read each section carefully and say the sample sentences aloud to build confidence and pronunciation. LexoraED modules are designed as microlessons: each section builds on the previous one so you never feel lost. Take notes in your own language first if that helps, then rewrite key ideas in English. Learning a language is a process, so celebrate small wins such as remembering one new phrase or fixing one grammar mistake during a role-play.

## Learning Objectives
By the end of this lesson you will be able to recognize key words and phrases, use them in simple sentences, answer short questions about the topic, and avoid common beginner mistakes. You will also practice listening for meaning, choosing appropriate responses, and reviewing what you learned with the module quiz. You should be able to explain the topic to a partner in one or two sentences, complete a short written exercise without copying from the screen, and identify at least three expressions that are appropriate in polite conversation. Finally, you will prepare for the twelve-question assessment that checks multiple choice, true or false, identification, and situational judgment skills related to this topic.

## Main Discussion
The main focus of Colors and Descriptions is colors and descriptions. Start with high-frequency expressions learners hear every day. Notice spelling, stress, and polite forms. When you speak, use short sentences: subject plus verb plus complement. Repeat new items in context instead of memorizing isolated lists. Pair words with gestures or pictures when possible. If you are unsure, ask a clarifying question such as Could you repeat that, please? Keep a small notebook for examples you personally need at school, work, or online study. Compare formal and informal registers when the lesson allows: some phrases fit friends, while others fit teachers or supervisors. Watch for false friends from your first language that look like English but mean something different. Record yourself for thirty seconds and listen for clarity. Discuss the topic with a study partner and correct each other kindly. When reading, underline new vocabulary and write a synonym you already know beside each word.

## Examples
Example 1: Learner A greets Learner B in the morning and asks a simple follow-up question. Example 2: Learner describes one fact using the target vocabulary in a complete sentence. Example 3: Learner listens to a short prompt and selects the best response from two options. Example 4: Learner rewrites a broken sentence into a correct and polite English line. Example 5: Two learners role-play a classroom scenario and swap roles so both speak and listen. Example 6: Learner writes three original sentences, checks them against the lesson notes, and corrects any tense or word-order errors before submitting practice work.

## Summary
You reviewed Colors and Descriptions by studying introduction, objectives, discussion, and examples. Continue practicing aloud for five minutes after each study session. Complete the quiz to check understanding, then revisit any missed questions and read the explanations. Strong learners review little and often rather than cramming once a week. Return to this module when you need a refresher before presentations, interviews, or exams. Share one new sentence with a friend or family member to reinforce memory. Mark the module complete in LexoraED only after you feel comfortable using the language without reading every word from the screen.",
            ModuleCategory.Vocabulary,
            DifficultyLevel.Beginner,
            5,
            4,
            new List<QuizQuestionDefinition>
            {
                new(QuizQuestionType.MultipleChoice, "Which option best matches the main idea of Colors and Descriptions?", "An unrelated science term", "A correct core concept", "A random number only", "A punctuation mark only", "B", "Choose the concept taught in the lesson."),
            new(QuizQuestionType.MultipleChoice, "When practicing Colors and Descriptions, what helps most?", "Memorizing with no context", "Using words in real sentences", "Avoiding all speaking", "Skipping review", "B"),
            new(QuizQuestionType.MultipleChoice, "Which sentence is grammatically appropriate for Colors and Descriptions?", "Me go store yesterday always", "She study careful every day", "They are learning step by step", "Him is go now quick", "C"),
            new(QuizQuestionType.MultipleChoice, "Select the polite response related to Colors and Descriptions.", "Go away now", "Could you repeat that, please?", "I no listen", "That stupid question", "B"),
            new(QuizQuestionType.MultipleChoice, "What should you do after studying Colors and Descriptions?", "Never practice again", "Practice aloud briefly", "Delete your notes", "Ignore the quiz", "B"),
            new(QuizQuestionType.MultipleChoice, "Which study habit supports Colors and Descriptions?", "Cram once a month only", "Review a few minutes often", "Skip all examples", "Avoid asking questions", "B"),
            new(QuizQuestionType.TrueFalse, "True or False: Connecting Colors and Descriptions to daily situations improves retention.", "True", "False", "", "", "True", "Context helps memory."),
            new(QuizQuestionType.TrueFalse, "True or False: You should never speak new words aloud when learning Colors and Descriptions.", "True", "False", "", "", "False", "Speaking supports pronunciation."),
            new(QuizQuestionType.Identification, "Fill in the blank: Good ______ is important when learning Colors and Descriptions.", "", "", "", "", "practice"),
            new(QuizQuestionType.Identification, "Write the missing word: Review your ______ after each lesson on Colors and Descriptions.", "", "", "", "", "notes"),
            new(QuizQuestionType.Situational, "You forget a word during a Colors and Descriptions activity. What is the best action?", "Stay silent forever", "Ask for clarification politely", "Leave the room angrily", "Change the subject rudely", "clarification|repeat|please"),
            new(QuizQuestionType.Situational, "A classmate needs help with Colors and Descriptions. What should you do?", "Mock their mistake", "Offer a short helpful example", "Ignore them completely", "Copy their work", "help|example|support")
            });
        yield return new CurriculumModuleDefinition(
            "Family Members",
            "Talk about relatives and relationships in English.",
            @"## Introduction
Welcome to Family Members. In this microlesson you will study family members through clear explanations, guided practice, and short examples you can use immediately in class or conversation. English becomes easier when you connect new words and patterns to situations you already know, such as meeting classmates, answering the teacher, or describing your day. Read each section carefully and say the sample sentences aloud to build confidence and pronunciation. LexoraED modules are designed as microlessons: each section builds on the previous one so you never feel lost. Take notes in your own language first if that helps, then rewrite key ideas in English. Learning a language is a process, so celebrate small wins such as remembering one new phrase or fixing one grammar mistake during a role-play.

## Learning Objectives
By the end of this lesson you will be able to recognize key words and phrases, use them in simple sentences, answer short questions about the topic, and avoid common beginner mistakes. You will also practice listening for meaning, choosing appropriate responses, and reviewing what you learned with the module quiz. You should be able to explain the topic to a partner in one or two sentences, complete a short written exercise without copying from the screen, and identify at least three expressions that are appropriate in polite conversation. Finally, you will prepare for the twelve-question assessment that checks multiple choice, true or false, identification, and situational judgment skills related to this topic.

## Main Discussion
The main focus of Family Members is family members. Start with high-frequency expressions learners hear every day. Notice spelling, stress, and polite forms. When you speak, use short sentences: subject plus verb plus complement. Repeat new items in context instead of memorizing isolated lists. Pair words with gestures or pictures when possible. If you are unsure, ask a clarifying question such as Could you repeat that, please? Keep a small notebook for examples you personally need at school, work, or online study. Compare formal and informal registers when the lesson allows: some phrases fit friends, while others fit teachers or supervisors. Watch for false friends from your first language that look like English but mean something different. Record yourself for thirty seconds and listen for clarity. Discuss the topic with a study partner and correct each other kindly. When reading, underline new vocabulary and write a synonym you already know beside each word.

## Examples
Example 1: Learner A greets Learner B in the morning and asks a simple follow-up question. Example 2: Learner describes one fact using the target vocabulary in a complete sentence. Example 3: Learner listens to a short prompt and selects the best response from two options. Example 4: Learner rewrites a broken sentence into a correct and polite English line. Example 5: Two learners role-play a classroom scenario and swap roles so both speak and listen. Example 6: Learner writes three original sentences, checks them against the lesson notes, and corrects any tense or word-order errors before submitting practice work.

## Summary
You reviewed Family Members by studying introduction, objectives, discussion, and examples. Continue practicing aloud for five minutes after each study session. Complete the quiz to check understanding, then revisit any missed questions and read the explanations. Strong learners review little and often rather than cramming once a week. Return to this module when you need a refresher before presentations, interviews, or exams. Share one new sentence with a friend or family member to reinforce memory. Mark the module complete in LexoraED only after you feel comfortable using the language without reading every word from the screen.",
            ModuleCategory.Vocabulary,
            DifficultyLevel.Beginner,
            6,
            5,
            new List<QuizQuestionDefinition>
            {
                new(QuizQuestionType.MultipleChoice, "Which option best matches the main idea of Family Members?", "An unrelated science term", "A correct core concept", "A random number only", "A punctuation mark only", "B", "Choose the concept taught in the lesson."),
            new(QuizQuestionType.MultipleChoice, "When practicing Family Members, what helps most?", "Memorizing with no context", "Using words in real sentences", "Avoiding all speaking", "Skipping review", "B"),
            new(QuizQuestionType.MultipleChoice, "Which sentence is grammatically appropriate for Family Members?", "Me go store yesterday always", "She study careful every day", "They are learning step by step", "Him is go now quick", "C"),
            new(QuizQuestionType.MultipleChoice, "Select the polite response related to Family Members.", "Go away now", "Could you repeat that, please?", "I no listen", "That stupid question", "B"),
            new(QuizQuestionType.MultipleChoice, "What should you do after studying Family Members?", "Never practice again", "Practice aloud briefly", "Delete your notes", "Ignore the quiz", "B"),
            new(QuizQuestionType.MultipleChoice, "Which study habit supports Family Members?", "Cram once a month only", "Review a few minutes often", "Skip all examples", "Avoid asking questions", "B"),
            new(QuizQuestionType.TrueFalse, "True or False: Connecting Family Members to daily situations improves retention.", "True", "False", "", "", "True", "Context helps memory."),
            new(QuizQuestionType.TrueFalse, "True or False: You should never speak new words aloud when learning Family Members.", "True", "False", "", "", "False", "Speaking supports pronunciation."),
            new(QuizQuestionType.Identification, "Fill in the blank: Good ______ is important when learning Family Members.", "", "", "", "", "practice"),
            new(QuizQuestionType.Identification, "Write the missing word: Review your ______ after each lesson on Family Members.", "", "", "", "", "notes"),
            new(QuizQuestionType.Situational, "You forget a word during a Family Members activity. What is the best action?", "Stay silent forever", "Ask for clarification politely", "Leave the room angrily", "Change the subject rudely", "clarification|repeat|please"),
            new(QuizQuestionType.Situational, "A classmate needs help with Family Members. What should you do?", "Mock their mistake", "Offer a short helpful example", "Ignore them completely", "Copy their work", "help|example|support")
            });
        yield return new CurriculumModuleDefinition(
            "Daily Activities",
            "Describe routines using common verbs and time expressions.",
            @"## Introduction
Welcome to Daily Activities. In this microlesson you will study daily activities through clear explanations, guided practice, and short examples you can use immediately in class or conversation. English becomes easier when you connect new words and patterns to situations you already know, such as meeting classmates, answering the teacher, or describing your day. Read each section carefully and say the sample sentences aloud to build confidence and pronunciation. LexoraED modules are designed as microlessons: each section builds on the previous one so you never feel lost. Take notes in your own language first if that helps, then rewrite key ideas in English. Learning a language is a process, so celebrate small wins such as remembering one new phrase or fixing one grammar mistake during a role-play.

## Learning Objectives
By the end of this lesson you will be able to recognize key words and phrases, use them in simple sentences, answer short questions about the topic, and avoid common beginner mistakes. You will also practice listening for meaning, choosing appropriate responses, and reviewing what you learned with the module quiz. You should be able to explain the topic to a partner in one or two sentences, complete a short written exercise without copying from the screen, and identify at least three expressions that are appropriate in polite conversation. Finally, you will prepare for the twelve-question assessment that checks multiple choice, true or false, identification, and situational judgment skills related to this topic.

## Main Discussion
The main focus of Daily Activities is daily activities. Start with high-frequency expressions learners hear every day. Notice spelling, stress, and polite forms. When you speak, use short sentences: subject plus verb plus complement. Repeat new items in context instead of memorizing isolated lists. Pair words with gestures or pictures when possible. If you are unsure, ask a clarifying question such as Could you repeat that, please? Keep a small notebook for examples you personally need at school, work, or online study. Compare formal and informal registers when the lesson allows: some phrases fit friends, while others fit teachers or supervisors. Watch for false friends from your first language that look like English but mean something different. Record yourself for thirty seconds and listen for clarity. Discuss the topic with a study partner and correct each other kindly. When reading, underline new vocabulary and write a synonym you already know beside each word.

## Examples
Example 1: Learner A greets Learner B in the morning and asks a simple follow-up question. Example 2: Learner describes one fact using the target vocabulary in a complete sentence. Example 3: Learner listens to a short prompt and selects the best response from two options. Example 4: Learner rewrites a broken sentence into a correct and polite English line. Example 5: Two learners role-play a classroom scenario and swap roles so both speak and listen. Example 6: Learner writes three original sentences, checks them against the lesson notes, and corrects any tense or word-order errors before submitting practice work.

## Summary
You reviewed Daily Activities by studying introduction, objectives, discussion, and examples. Continue practicing aloud for five minutes after each study session. Complete the quiz to check understanding, then revisit any missed questions and read the explanations. Strong learners review little and often rather than cramming once a week. Return to this module when you need a refresher before presentations, interviews, or exams. Share one new sentence with a friend or family member to reinforce memory. Mark the module complete in LexoraED only after you feel comfortable using the language without reading every word from the screen.",
            ModuleCategory.Grammar,
            DifficultyLevel.Beginner,
            7,
            6,
            new List<QuizQuestionDefinition>
            {
                new(QuizQuestionType.MultipleChoice, "Which option best matches the main idea of Daily Activities?", "An unrelated science term", "A correct core concept", "A random number only", "A punctuation mark only", "B", "Choose the concept taught in the lesson."),
            new(QuizQuestionType.MultipleChoice, "When practicing Daily Activities, what helps most?", "Memorizing with no context", "Using words in real sentences", "Avoiding all speaking", "Skipping review", "B"),
            new(QuizQuestionType.MultipleChoice, "Which sentence is grammatically appropriate for Daily Activities?", "Me go store yesterday always", "She study careful every day", "They are learning step by step", "Him is go now quick", "C"),
            new(QuizQuestionType.MultipleChoice, "Select the polite response related to Daily Activities.", "Go away now", "Could you repeat that, please?", "I no listen", "That stupid question", "B"),
            new(QuizQuestionType.MultipleChoice, "What should you do after studying Daily Activities?", "Never practice again", "Practice aloud briefly", "Delete your notes", "Ignore the quiz", "B"),
            new(QuizQuestionType.MultipleChoice, "Which study habit supports Daily Activities?", "Cram once a month only", "Review a few minutes often", "Skip all examples", "Avoid asking questions", "B"),
            new(QuizQuestionType.TrueFalse, "True or False: Connecting Daily Activities to daily situations improves retention.", "True", "False", "", "", "True", "Context helps memory."),
            new(QuizQuestionType.TrueFalse, "True or False: You should never speak new words aloud when learning Daily Activities.", "True", "False", "", "", "False", "Speaking supports pronunciation."),
            new(QuizQuestionType.Identification, "Fill in the blank: Good ______ is important when learning Daily Activities.", "", "", "", "", "practice"),
            new(QuizQuestionType.Identification, "Write the missing word: Review your ______ after each lesson on Daily Activities.", "", "", "", "", "notes"),
            new(QuizQuestionType.Situational, "You forget a word during a Daily Activities activity. What is the best action?", "Stay silent forever", "Ask for clarification politely", "Leave the room angrily", "Change the subject rudely", "clarification|repeat|please"),
            new(QuizQuestionType.Situational, "A classmate needs help with Daily Activities. What should you do?", "Mock their mistake", "Offer a short helpful example", "Ignore them completely", "Copy their work", "help|example|support")
            });    }

    private static IEnumerable<CurriculumModuleDefinition> IntermediateModules()
    {
        yield return new CurriculumModuleDefinition(
            "Sentence Construction",
            "Build clear sentences with subjects, verbs, and complements.",
            @"## Introduction
Welcome to Sentence Construction. In this microlesson you will study sentence construction through clear explanations, guided practice, and short examples you can use immediately in class or conversation. English becomes easier when you connect new words and patterns to situations you already know, such as meeting classmates, answering the teacher, or describing your day. Read each section carefully and say the sample sentences aloud to build confidence and pronunciation. LexoraED modules are designed as microlessons: each section builds on the previous one so you never feel lost. Take notes in your own language first if that helps, then rewrite key ideas in English. Learning a language is a process, so celebrate small wins such as remembering one new phrase or fixing one grammar mistake during a role-play.

## Learning Objectives
By the end of this lesson you will be able to recognize key words and phrases, use them in simple sentences, answer short questions about the topic, and avoid common beginner mistakes. You will also practice listening for meaning, choosing appropriate responses, and reviewing what you learned with the module quiz. You should be able to explain the topic to a partner in one or two sentences, complete a short written exercise without copying from the screen, and identify at least three expressions that are appropriate in polite conversation. Finally, you will prepare for the twelve-question assessment that checks multiple choice, true or false, identification, and situational judgment skills related to this topic.

## Main Discussion
The main focus of Sentence Construction is sentence construction. Start with high-frequency expressions learners hear every day. Notice spelling, stress, and polite forms. When you speak, use short sentences: subject plus verb plus complement. Repeat new items in context instead of memorizing isolated lists. Pair words with gestures or pictures when possible. If you are unsure, ask a clarifying question such as Could you repeat that, please? Keep a small notebook for examples you personally need at school, work, or online study. Compare formal and informal registers when the lesson allows: some phrases fit friends, while others fit teachers or supervisors. Watch for false friends from your first language that look like English but mean something different. Record yourself for thirty seconds and listen for clarity. Discuss the topic with a study partner and correct each other kindly. When reading, underline new vocabulary and write a synonym you already know beside each word.

## Examples
Example 1: Learner A greets Learner B in the morning and asks a simple follow-up question. Example 2: Learner describes one fact using the target vocabulary in a complete sentence. Example 3: Learner listens to a short prompt and selects the best response from two options. Example 4: Learner rewrites a broken sentence into a correct and polite English line. Example 5: Two learners role-play a classroom scenario and swap roles so both speak and listen. Example 6: Learner writes three original sentences, checks them against the lesson notes, and corrects any tense or word-order errors before submitting practice work.

## Summary
You reviewed Sentence Construction by studying introduction, objectives, discussion, and examples. Continue practicing aloud for five minutes after each study session. Complete the quiz to check understanding, then revisit any missed questions and read the explanations. Strong learners review little and often rather than cramming once a week. Return to this module when you need a refresher before presentations, interviews, or exams. Share one new sentence with a friend or family member to reinforce memory. Mark the module complete in LexoraED only after you feel comfortable using the language without reading every word from the screen.",
            ModuleCategory.Grammar,
            DifficultyLevel.Intermediate,
            1,
            null,
            new List<QuizQuestionDefinition>
            {
                new(QuizQuestionType.MultipleChoice, "Which option best matches the main idea of Sentence Construction?", "An unrelated science term", "A correct core concept", "A random number only", "A punctuation mark only", "B", "Choose the concept taught in the lesson."),
            new(QuizQuestionType.MultipleChoice, "When practicing Sentence Construction, what helps most?", "Memorizing with no context", "Using words in real sentences", "Avoiding all speaking", "Skipping review", "B"),
            new(QuizQuestionType.MultipleChoice, "Which sentence is grammatically appropriate for Sentence Construction?", "Me go store yesterday always", "She study careful every day", "They are learning step by step", "Him is go now quick", "C"),
            new(QuizQuestionType.MultipleChoice, "Select the polite response related to Sentence Construction.", "Go away now", "Could you repeat that, please?", "I no listen", "That stupid question", "B"),
            new(QuizQuestionType.MultipleChoice, "What should you do after studying Sentence Construction?", "Never practice again", "Practice aloud briefly", "Delete your notes", "Ignore the quiz", "B"),
            new(QuizQuestionType.MultipleChoice, "Which study habit supports Sentence Construction?", "Cram once a month only", "Review a few minutes often", "Skip all examples", "Avoid asking questions", "B"),
            new(QuizQuestionType.TrueFalse, "True or False: Connecting Sentence Construction to daily situations improves retention.", "True", "False", "", "", "True", "Context helps memory."),
            new(QuizQuestionType.TrueFalse, "True or False: You should never speak new words aloud when learning Sentence Construction.", "True", "False", "", "", "False", "Speaking supports pronunciation."),
            new(QuizQuestionType.Identification, "Fill in the blank: Good ______ is important when learning Sentence Construction.", "", "", "", "", "practice"),
            new(QuizQuestionType.Identification, "Write the missing word: Review your ______ after each lesson on Sentence Construction.", "", "", "", "", "notes"),
            new(QuizQuestionType.Situational, "You forget a word during a Sentence Construction activity. What is the best action?", "Stay silent forever", "Ask for clarification politely", "Leave the room angrily", "Change the subject rudely", "clarification|repeat|please"),
            new(QuizQuestionType.Situational, "A classmate needs help with Sentence Construction. What should you do?", "Mock their mistake", "Offer a short helpful example", "Ignore them completely", "Copy their work", "help|example|support")
            });
        yield return new CurriculumModuleDefinition(
            "Verb Tenses",
            "Use present, past, and future forms accurately in context.",
            @"## Introduction
Welcome to Verb Tenses. In this microlesson you will study verb tenses through clear explanations, guided practice, and short examples you can use immediately in class or conversation. English becomes easier when you connect new words and patterns to situations you already know, such as meeting classmates, answering the teacher, or describing your day. Read each section carefully and say the sample sentences aloud to build confidence and pronunciation. LexoraED modules are designed as microlessons: each section builds on the previous one so you never feel lost. Take notes in your own language first if that helps, then rewrite key ideas in English. Learning a language is a process, so celebrate small wins such as remembering one new phrase or fixing one grammar mistake during a role-play.

## Learning Objectives
By the end of this lesson you will be able to recognize key words and phrases, use them in simple sentences, answer short questions about the topic, and avoid common beginner mistakes. You will also practice listening for meaning, choosing appropriate responses, and reviewing what you learned with the module quiz. You should be able to explain the topic to a partner in one or two sentences, complete a short written exercise without copying from the screen, and identify at least three expressions that are appropriate in polite conversation. Finally, you will prepare for the twelve-question assessment that checks multiple choice, true or false, identification, and situational judgment skills related to this topic.

## Main Discussion
The main focus of Verb Tenses is verb tenses. Start with high-frequency expressions learners hear every day. Notice spelling, stress, and polite forms. When you speak, use short sentences: subject plus verb plus complement. Repeat new items in context instead of memorizing isolated lists. Pair words with gestures or pictures when possible. If you are unsure, ask a clarifying question such as Could you repeat that, please? Keep a small notebook for examples you personally need at school, work, or online study. Compare formal and informal registers when the lesson allows: some phrases fit friends, while others fit teachers or supervisors. Watch for false friends from your first language that look like English but mean something different. Record yourself for thirty seconds and listen for clarity. Discuss the topic with a study partner and correct each other kindly. When reading, underline new vocabulary and write a synonym you already know beside each word.

## Examples
Example 1: Learner A greets Learner B in the morning and asks a simple follow-up question. Example 2: Learner describes one fact using the target vocabulary in a complete sentence. Example 3: Learner listens to a short prompt and selects the best response from two options. Example 4: Learner rewrites a broken sentence into a correct and polite English line. Example 5: Two learners role-play a classroom scenario and swap roles so both speak and listen. Example 6: Learner writes three original sentences, checks them against the lesson notes, and corrects any tense or word-order errors before submitting practice work.

## Summary
You reviewed Verb Tenses by studying introduction, objectives, discussion, and examples. Continue practicing aloud for five minutes after each study session. Complete the quiz to check understanding, then revisit any missed questions and read the explanations. Strong learners review little and often rather than cramming once a week. Return to this module when you need a refresher before presentations, interviews, or exams. Share one new sentence with a friend or family member to reinforce memory. Mark the module complete in LexoraED only after you feel comfortable using the language without reading every word from the screen.",
            ModuleCategory.Grammar,
            DifficultyLevel.Intermediate,
            2,
            1,
            new List<QuizQuestionDefinition>
            {
                new(QuizQuestionType.MultipleChoice, "Which option best matches the main idea of Verb Tenses?", "An unrelated science term", "A correct core concept", "A random number only", "A punctuation mark only", "B", "Choose the concept taught in the lesson."),
            new(QuizQuestionType.MultipleChoice, "When practicing Verb Tenses, what helps most?", "Memorizing with no context", "Using words in real sentences", "Avoiding all speaking", "Skipping review", "B"),
            new(QuizQuestionType.MultipleChoice, "Which sentence is grammatically appropriate for Verb Tenses?", "Me go store yesterday always", "She study careful every day", "They are learning step by step", "Him is go now quick", "C"),
            new(QuizQuestionType.MultipleChoice, "Select the polite response related to Verb Tenses.", "Go away now", "Could you repeat that, please?", "I no listen", "That stupid question", "B"),
            new(QuizQuestionType.MultipleChoice, "What should you do after studying Verb Tenses?", "Never practice again", "Practice aloud briefly", "Delete your notes", "Ignore the quiz", "B"),
            new(QuizQuestionType.MultipleChoice, "Which study habit supports Verb Tenses?", "Cram once a month only", "Review a few minutes often", "Skip all examples", "Avoid asking questions", "B"),
            new(QuizQuestionType.TrueFalse, "True or False: Connecting Verb Tenses to daily situations improves retention.", "True", "False", "", "", "True", "Context helps memory."),
            new(QuizQuestionType.TrueFalse, "True or False: You should never speak new words aloud when learning Verb Tenses.", "True", "False", "", "", "False", "Speaking supports pronunciation."),
            new(QuizQuestionType.Identification, "Fill in the blank: Good ______ is important when learning Verb Tenses.", "", "", "", "", "practice"),
            new(QuizQuestionType.Identification, "Write the missing word: Review your ______ after each lesson on Verb Tenses.", "", "", "", "", "notes"),
            new(QuizQuestionType.Situational, "You forget a word during a Verb Tenses activity. What is the best action?", "Stay silent forever", "Ask for clarification politely", "Leave the room angrily", "Change the subject rudely", "clarification|repeat|please"),
            new(QuizQuestionType.Situational, "A classmate needs help with Verb Tenses. What should you do?", "Mock their mistake", "Offer a short helpful example", "Ignore them completely", "Copy their work", "help|example|support")
            });
        yield return new CurriculumModuleDefinition(
            "Reading Comprehension",
            "Identify main ideas and details in short texts.",
            @"## Introduction
Welcome to Reading Comprehension. In this microlesson you will study reading comprehension through clear explanations, guided practice, and short examples you can use immediately in class or conversation. English becomes easier when you connect new words and patterns to situations you already know, such as meeting classmates, answering the teacher, or describing your day. Read each section carefully and say the sample sentences aloud to build confidence and pronunciation. LexoraED modules are designed as microlessons: each section builds on the previous one so you never feel lost. Take notes in your own language first if that helps, then rewrite key ideas in English. Learning a language is a process, so celebrate small wins such as remembering one new phrase or fixing one grammar mistake during a role-play.

## Learning Objectives
By the end of this lesson you will be able to recognize key words and phrases, use them in simple sentences, answer short questions about the topic, and avoid common beginner mistakes. You will also practice listening for meaning, choosing appropriate responses, and reviewing what you learned with the module quiz. You should be able to explain the topic to a partner in one or two sentences, complete a short written exercise without copying from the screen, and identify at least three expressions that are appropriate in polite conversation. Finally, you will prepare for the twelve-question assessment that checks multiple choice, true or false, identification, and situational judgment skills related to this topic.

## Main Discussion
The main focus of Reading Comprehension is reading comprehension. Start with high-frequency expressions learners hear every day. Notice spelling, stress, and polite forms. When you speak, use short sentences: subject plus verb plus complement. Repeat new items in context instead of memorizing isolated lists. Pair words with gestures or pictures when possible. If you are unsure, ask a clarifying question such as Could you repeat that, please? Keep a small notebook for examples you personally need at school, work, or online study. Compare formal and informal registers when the lesson allows: some phrases fit friends, while others fit teachers or supervisors. Watch for false friends from your first language that look like English but mean something different. Record yourself for thirty seconds and listen for clarity. Discuss the topic with a study partner and correct each other kindly. When reading, underline new vocabulary and write a synonym you already know beside each word.

## Examples
Example 1: Learner A greets Learner B in the morning and asks a simple follow-up question. Example 2: Learner describes one fact using the target vocabulary in a complete sentence. Example 3: Learner listens to a short prompt and selects the best response from two options. Example 4: Learner rewrites a broken sentence into a correct and polite English line. Example 5: Two learners role-play a classroom scenario and swap roles so both speak and listen. Example 6: Learner writes three original sentences, checks them against the lesson notes, and corrects any tense or word-order errors before submitting practice work.

## Summary
You reviewed Reading Comprehension by studying introduction, objectives, discussion, and examples. Continue practicing aloud for five minutes after each study session. Complete the quiz to check understanding, then revisit any missed questions and read the explanations. Strong learners review little and often rather than cramming once a week. Return to this module when you need a refresher before presentations, interviews, or exams. Share one new sentence with a friend or family member to reinforce memory. Mark the module complete in LexoraED only after you feel comfortable using the language without reading every word from the screen.",
            ModuleCategory.Reading,
            DifficultyLevel.Intermediate,
            3,
            2,
            new List<QuizQuestionDefinition>
            {
                new(QuizQuestionType.MultipleChoice, "Which option best matches the main idea of Reading Comprehension?", "An unrelated science term", "A correct core concept", "A random number only", "A punctuation mark only", "B", "Choose the concept taught in the lesson."),
            new(QuizQuestionType.MultipleChoice, "When practicing Reading Comprehension, what helps most?", "Memorizing with no context", "Using words in real sentences", "Avoiding all speaking", "Skipping review", "B"),
            new(QuizQuestionType.MultipleChoice, "Which sentence is grammatically appropriate for Reading Comprehension?", "Me go store yesterday always", "She study careful every day", "They are learning step by step", "Him is go now quick", "C"),
            new(QuizQuestionType.MultipleChoice, "Select the polite response related to Reading Comprehension.", "Go away now", "Could you repeat that, please?", "I no listen", "That stupid question", "B"),
            new(QuizQuestionType.MultipleChoice, "What should you do after studying Reading Comprehension?", "Never practice again", "Practice aloud briefly", "Delete your notes", "Ignore the quiz", "B"),
            new(QuizQuestionType.MultipleChoice, "Which study habit supports Reading Comprehension?", "Cram once a month only", "Review a few minutes often", "Skip all examples", "Avoid asking questions", "B"),
            new(QuizQuestionType.TrueFalse, "True or False: Connecting Reading Comprehension to daily situations improves retention.", "True", "False", "", "", "True", "Context helps memory."),
            new(QuizQuestionType.TrueFalse, "True or False: You should never speak new words aloud when learning Reading Comprehension.", "True", "False", "", "", "False", "Speaking supports pronunciation."),
            new(QuizQuestionType.Identification, "Fill in the blank: Good ______ is important when learning Reading Comprehension.", "", "", "", "", "practice"),
            new(QuizQuestionType.Identification, "Write the missing word: Review your ______ after each lesson on Reading Comprehension.", "", "", "", "", "notes"),
            new(QuizQuestionType.Situational, "You forget a word during a Reading Comprehension activity. What is the best action?", "Stay silent forever", "Ask for clarification politely", "Leave the room angrily", "Change the subject rudely", "clarification|repeat|please"),
            new(QuizQuestionType.Situational, "A classmate needs help with Reading Comprehension. What should you do?", "Mock their mistake", "Offer a short helpful example", "Ignore them completely", "Copy their work", "help|example|support")
            });
        yield return new CurriculumModuleDefinition(
            "Short Paragraph Writing",
            "Organize ideas into coherent short paragraphs.",
            @"## Introduction
Welcome to Short Paragraph Writing. In this microlesson you will study short paragraph writing through clear explanations, guided practice, and short examples you can use immediately in class or conversation. English becomes easier when you connect new words and patterns to situations you already know, such as meeting classmates, answering the teacher, or describing your day. Read each section carefully and say the sample sentences aloud to build confidence and pronunciation. LexoraED modules are designed as microlessons: each section builds on the previous one so you never feel lost. Take notes in your own language first if that helps, then rewrite key ideas in English. Learning a language is a process, so celebrate small wins such as remembering one new phrase or fixing one grammar mistake during a role-play.

## Learning Objectives
By the end of this lesson you will be able to recognize key words and phrases, use them in simple sentences, answer short questions about the topic, and avoid common beginner mistakes. You will also practice listening for meaning, choosing appropriate responses, and reviewing what you learned with the module quiz. You should be able to explain the topic to a partner in one or two sentences, complete a short written exercise without copying from the screen, and identify at least three expressions that are appropriate in polite conversation. Finally, you will prepare for the twelve-question assessment that checks multiple choice, true or false, identification, and situational judgment skills related to this topic.

## Main Discussion
The main focus of Short Paragraph Writing is short paragraph writing. Start with high-frequency expressions learners hear every day. Notice spelling, stress, and polite forms. When you speak, use short sentences: subject plus verb plus complement. Repeat new items in context instead of memorizing isolated lists. Pair words with gestures or pictures when possible. If you are unsure, ask a clarifying question such as Could you repeat that, please? Keep a small notebook for examples you personally need at school, work, or online study. Compare formal and informal registers when the lesson allows: some phrases fit friends, while others fit teachers or supervisors. Watch for false friends from your first language that look like English but mean something different. Record yourself for thirty seconds and listen for clarity. Discuss the topic with a study partner and correct each other kindly. When reading, underline new vocabulary and write a synonym you already know beside each word.

## Examples
Example 1: Learner A greets Learner B in the morning and asks a simple follow-up question. Example 2: Learner describes one fact using the target vocabulary in a complete sentence. Example 3: Learner listens to a short prompt and selects the best response from two options. Example 4: Learner rewrites a broken sentence into a correct and polite English line. Example 5: Two learners role-play a classroom scenario and swap roles so both speak and listen. Example 6: Learner writes three original sentences, checks them against the lesson notes, and corrects any tense or word-order errors before submitting practice work.

## Summary
You reviewed Short Paragraph Writing by studying introduction, objectives, discussion, and examples. Continue practicing aloud for five minutes after each study session. Complete the quiz to check understanding, then revisit any missed questions and read the explanations. Strong learners review little and often rather than cramming once a week. Return to this module when you need a refresher before presentations, interviews, or exams. Share one new sentence with a friend or family member to reinforce memory. Mark the module complete in LexoraED only after you feel comfortable using the language without reading every word from the screen.",
            ModuleCategory.Reading,
            DifficultyLevel.Intermediate,
            4,
            3,
            new List<QuizQuestionDefinition>
            {
                new(QuizQuestionType.MultipleChoice, "Which option best matches the main idea of Short Paragraph Writing?", "An unrelated science term", "A correct core concept", "A random number only", "A punctuation mark only", "B", "Choose the concept taught in the lesson."),
            new(QuizQuestionType.MultipleChoice, "When practicing Short Paragraph Writing, what helps most?", "Memorizing with no context", "Using words in real sentences", "Avoiding all speaking", "Skipping review", "B"),
            new(QuizQuestionType.MultipleChoice, "Which sentence is grammatically appropriate for Short Paragraph Writing?", "Me go store yesterday always", "She study careful every day", "They are learning step by step", "Him is go now quick", "C"),
            new(QuizQuestionType.MultipleChoice, "Select the polite response related to Short Paragraph Writing.", "Go away now", "Could you repeat that, please?", "I no listen", "That stupid question", "B"),
            new(QuizQuestionType.MultipleChoice, "What should you do after studying Short Paragraph Writing?", "Never practice again", "Practice aloud briefly", "Delete your notes", "Ignore the quiz", "B"),
            new(QuizQuestionType.MultipleChoice, "Which study habit supports Short Paragraph Writing?", "Cram once a month only", "Review a few minutes often", "Skip all examples", "Avoid asking questions", "B"),
            new(QuizQuestionType.TrueFalse, "True or False: Connecting Short Paragraph Writing to daily situations improves retention.", "True", "False", "", "", "True", "Context helps memory."),
            new(QuizQuestionType.TrueFalse, "True or False: You should never speak new words aloud when learning Short Paragraph Writing.", "True", "False", "", "", "False", "Speaking supports pronunciation."),
            new(QuizQuestionType.Identification, "Fill in the blank: Good ______ is important when learning Short Paragraph Writing.", "", "", "", "", "practice"),
            new(QuizQuestionType.Identification, "Write the missing word: Review your ______ after each lesson on Short Paragraph Writing.", "", "", "", "", "notes"),
            new(QuizQuestionType.Situational, "You forget a word during a Short Paragraph Writing activity. What is the best action?", "Stay silent forever", "Ask for clarification politely", "Leave the room angrily", "Change the subject rudely", "clarification|repeat|please"),
            new(QuizQuestionType.Situational, "A classmate needs help with Short Paragraph Writing. What should you do?", "Mock their mistake", "Offer a short helpful example", "Ignore them completely", "Copy their work", "help|example|support")
            });
        yield return new CurriculumModuleDefinition(
            "Pronouns",
            "Replace nouns correctly with personal and possessive pronouns.",
            @"## Introduction
Welcome to Pronouns. In this microlesson you will study pronouns through clear explanations, guided practice, and short examples you can use immediately in class or conversation. English becomes easier when you connect new words and patterns to situations you already know, such as meeting classmates, answering the teacher, or describing your day. Read each section carefully and say the sample sentences aloud to build confidence and pronunciation. LexoraED modules are designed as microlessons: each section builds on the previous one so you never feel lost. Take notes in your own language first if that helps, then rewrite key ideas in English. Learning a language is a process, so celebrate small wins such as remembering one new phrase or fixing one grammar mistake during a role-play.

## Learning Objectives
By the end of this lesson you will be able to recognize key words and phrases, use them in simple sentences, answer short questions about the topic, and avoid common beginner mistakes. You will also practice listening for meaning, choosing appropriate responses, and reviewing what you learned with the module quiz. You should be able to explain the topic to a partner in one or two sentences, complete a short written exercise without copying from the screen, and identify at least three expressions that are appropriate in polite conversation. Finally, you will prepare for the twelve-question assessment that checks multiple choice, true or false, identification, and situational judgment skills related to this topic.

## Main Discussion
The main focus of Pronouns is pronouns. Start with high-frequency expressions learners hear every day. Notice spelling, stress, and polite forms. When you speak, use short sentences: subject plus verb plus complement. Repeat new items in context instead of memorizing isolated lists. Pair words with gestures or pictures when possible. If you are unsure, ask a clarifying question such as Could you repeat that, please? Keep a small notebook for examples you personally need at school, work, or online study. Compare formal and informal registers when the lesson allows: some phrases fit friends, while others fit teachers or supervisors. Watch for false friends from your first language that look like English but mean something different. Record yourself for thirty seconds and listen for clarity. Discuss the topic with a study partner and correct each other kindly. When reading, underline new vocabulary and write a synonym you already know beside each word.

## Examples
Example 1: Learner A greets Learner B in the morning and asks a simple follow-up question. Example 2: Learner describes one fact using the target vocabulary in a complete sentence. Example 3: Learner listens to a short prompt and selects the best response from two options. Example 4: Learner rewrites a broken sentence into a correct and polite English line. Example 5: Two learners role-play a classroom scenario and swap roles so both speak and listen. Example 6: Learner writes three original sentences, checks them against the lesson notes, and corrects any tense or word-order errors before submitting practice work.

## Summary
You reviewed Pronouns by studying introduction, objectives, discussion, and examples. Continue practicing aloud for five minutes after each study session. Complete the quiz to check understanding, then revisit any missed questions and read the explanations. Strong learners review little and often rather than cramming once a week. Return to this module when you need a refresher before presentations, interviews, or exams. Share one new sentence with a friend or family member to reinforce memory. Mark the module complete in LexoraED only after you feel comfortable using the language without reading every word from the screen.",
            ModuleCategory.Grammar,
            DifficultyLevel.Intermediate,
            5,
            4,
            new List<QuizQuestionDefinition>
            {
                new(QuizQuestionType.MultipleChoice, "Which option best matches the main idea of Pronouns?", "An unrelated science term", "A correct core concept", "A random number only", "A punctuation mark only", "B", "Choose the concept taught in the lesson."),
            new(QuizQuestionType.MultipleChoice, "When practicing Pronouns, what helps most?", "Memorizing with no context", "Using words in real sentences", "Avoiding all speaking", "Skipping review", "B"),
            new(QuizQuestionType.MultipleChoice, "Which sentence is grammatically appropriate for Pronouns?", "Me go store yesterday always", "She study careful every day", "They are learning step by step", "Him is go now quick", "C"),
            new(QuizQuestionType.MultipleChoice, "Select the polite response related to Pronouns.", "Go away now", "Could you repeat that, please?", "I no listen", "That stupid question", "B"),
            new(QuizQuestionType.MultipleChoice, "What should you do after studying Pronouns?", "Never practice again", "Practice aloud briefly", "Delete your notes", "Ignore the quiz", "B"),
            new(QuizQuestionType.MultipleChoice, "Which study habit supports Pronouns?", "Cram once a month only", "Review a few minutes often", "Skip all examples", "Avoid asking questions", "B"),
            new(QuizQuestionType.TrueFalse, "True or False: Connecting Pronouns to daily situations improves retention.", "True", "False", "", "", "True", "Context helps memory."),
            new(QuizQuestionType.TrueFalse, "True or False: You should never speak new words aloud when learning Pronouns.", "True", "False", "", "", "False", "Speaking supports pronunciation."),
            new(QuizQuestionType.Identification, "Fill in the blank: Good ______ is important when learning Pronouns.", "", "", "", "", "practice"),
            new(QuizQuestionType.Identification, "Write the missing word: Review your ______ after each lesson on Pronouns.", "", "", "", "", "notes"),
            new(QuizQuestionType.Situational, "You forget a word during a Pronouns activity. What is the best action?", "Stay silent forever", "Ask for clarification politely", "Leave the room angrily", "Change the subject rudely", "clarification|repeat|please"),
            new(QuizQuestionType.Situational, "A classmate needs help with Pronouns. What should you do?", "Mock their mistake", "Offer a short helpful example", "Ignore them completely", "Copy their work", "help|example|support")
            });
        yield return new CurriculumModuleDefinition(
            "Adjectives",
            "Describe nouns with appropriate adjective forms and order.",
            @"## Introduction
Welcome to Adjectives. In this microlesson you will study adjectives through clear explanations, guided practice, and short examples you can use immediately in class or conversation. English becomes easier when you connect new words and patterns to situations you already know, such as meeting classmates, answering the teacher, or describing your day. Read each section carefully and say the sample sentences aloud to build confidence and pronunciation. LexoraED modules are designed as microlessons: each section builds on the previous one so you never feel lost. Take notes in your own language first if that helps, then rewrite key ideas in English. Learning a language is a process, so celebrate small wins such as remembering one new phrase or fixing one grammar mistake during a role-play.

## Learning Objectives
By the end of this lesson you will be able to recognize key words and phrases, use them in simple sentences, answer short questions about the topic, and avoid common beginner mistakes. You will also practice listening for meaning, choosing appropriate responses, and reviewing what you learned with the module quiz. You should be able to explain the topic to a partner in one or two sentences, complete a short written exercise without copying from the screen, and identify at least three expressions that are appropriate in polite conversation. Finally, you will prepare for the twelve-question assessment that checks multiple choice, true or false, identification, and situational judgment skills related to this topic.

## Main Discussion
The main focus of Adjectives is adjectives. Start with high-frequency expressions learners hear every day. Notice spelling, stress, and polite forms. When you speak, use short sentences: subject plus verb plus complement. Repeat new items in context instead of memorizing isolated lists. Pair words with gestures or pictures when possible. If you are unsure, ask a clarifying question such as Could you repeat that, please? Keep a small notebook for examples you personally need at school, work, or online study. Compare formal and informal registers when the lesson allows: some phrases fit friends, while others fit teachers or supervisors. Watch for false friends from your first language that look like English but mean something different. Record yourself for thirty seconds and listen for clarity. Discuss the topic with a study partner and correct each other kindly. When reading, underline new vocabulary and write a synonym you already know beside each word.

## Examples
Example 1: Learner A greets Learner B in the morning and asks a simple follow-up question. Example 2: Learner describes one fact using the target vocabulary in a complete sentence. Example 3: Learner listens to a short prompt and selects the best response from two options. Example 4: Learner rewrites a broken sentence into a correct and polite English line. Example 5: Two learners role-play a classroom scenario and swap roles so both speak and listen. Example 6: Learner writes three original sentences, checks them against the lesson notes, and corrects any tense or word-order errors before submitting practice work.

## Summary
You reviewed Adjectives by studying introduction, objectives, discussion, and examples. Continue practicing aloud for five minutes after each study session. Complete the quiz to check understanding, then revisit any missed questions and read the explanations. Strong learners review little and often rather than cramming once a week. Return to this module when you need a refresher before presentations, interviews, or exams. Share one new sentence with a friend or family member to reinforce memory. Mark the module complete in LexoraED only after you feel comfortable using the language without reading every word from the screen.",
            ModuleCategory.Grammar,
            DifficultyLevel.Intermediate,
            6,
            5,
            new List<QuizQuestionDefinition>
            {
                new(QuizQuestionType.MultipleChoice, "Which option best matches the main idea of Adjectives?", "An unrelated science term", "A correct core concept", "A random number only", "A punctuation mark only", "B", "Choose the concept taught in the lesson."),
            new(QuizQuestionType.MultipleChoice, "When practicing Adjectives, what helps most?", "Memorizing with no context", "Using words in real sentences", "Avoiding all speaking", "Skipping review", "B"),
            new(QuizQuestionType.MultipleChoice, "Which sentence is grammatically appropriate for Adjectives?", "Me go store yesterday always", "She study careful every day", "They are learning step by step", "Him is go now quick", "C"),
            new(QuizQuestionType.MultipleChoice, "Select the polite response related to Adjectives.", "Go away now", "Could you repeat that, please?", "I no listen", "That stupid question", "B"),
            new(QuizQuestionType.MultipleChoice, "What should you do after studying Adjectives?", "Never practice again", "Practice aloud briefly", "Delete your notes", "Ignore the quiz", "B"),
            new(QuizQuestionType.MultipleChoice, "Which study habit supports Adjectives?", "Cram once a month only", "Review a few minutes often", "Skip all examples", "Avoid asking questions", "B"),
            new(QuizQuestionType.TrueFalse, "True or False: Connecting Adjectives to daily situations improves retention.", "True", "False", "", "", "True", "Context helps memory."),
            new(QuizQuestionType.TrueFalse, "True or False: You should never speak new words aloud when learning Adjectives.", "True", "False", "", "", "False", "Speaking supports pronunciation."),
            new(QuizQuestionType.Identification, "Fill in the blank: Good ______ is important when learning Adjectives.", "", "", "", "", "practice"),
            new(QuizQuestionType.Identification, "Write the missing word: Review your ______ after each lesson on Adjectives.", "", "", "", "", "notes"),
            new(QuizQuestionType.Situational, "You forget a word during a Adjectives activity. What is the best action?", "Stay silent forever", "Ask for clarification politely", "Leave the room angrily", "Change the subject rudely", "clarification|repeat|please"),
            new(QuizQuestionType.Situational, "A classmate needs help with Adjectives. What should you do?", "Mock their mistake", "Offer a short helpful example", "Ignore them completely", "Copy their work", "help|example|support")
            });    }

    private static IEnumerable<CurriculumModuleDefinition> AdvancedModules()
    {
        yield return new CurriculumModuleDefinition(
            "Essay Writing",
            "Plan, draft, and revise short academic essays.",
            @"## Introduction
Welcome to Essay Writing. In this microlesson you will study essay writing through clear explanations, guided practice, and short examples you can use immediately in class or conversation. English becomes easier when you connect new words and patterns to situations you already know, such as meeting classmates, answering the teacher, or describing your day. Read each section carefully and say the sample sentences aloud to build confidence and pronunciation. LexoraED modules are designed as microlessons: each section builds on the previous one so you never feel lost. Take notes in your own language first if that helps, then rewrite key ideas in English. Learning a language is a process, so celebrate small wins such as remembering one new phrase or fixing one grammar mistake during a role-play.

## Learning Objectives
By the end of this lesson you will be able to recognize key words and phrases, use them in simple sentences, answer short questions about the topic, and avoid common beginner mistakes. You will also practice listening for meaning, choosing appropriate responses, and reviewing what you learned with the module quiz. You should be able to explain the topic to a partner in one or two sentences, complete a short written exercise without copying from the screen, and identify at least three expressions that are appropriate in polite conversation. Finally, you will prepare for the twelve-question assessment that checks multiple choice, true or false, identification, and situational judgment skills related to this topic.

## Main Discussion
The main focus of Essay Writing is essay writing. Start with high-frequency expressions learners hear every day. Notice spelling, stress, and polite forms. When you speak, use short sentences: subject plus verb plus complement. Repeat new items in context instead of memorizing isolated lists. Pair words with gestures or pictures when possible. If you are unsure, ask a clarifying question such as Could you repeat that, please? Keep a small notebook for examples you personally need at school, work, or online study. Compare formal and informal registers when the lesson allows: some phrases fit friends, while others fit teachers or supervisors. Watch for false friends from your first language that look like English but mean something different. Record yourself for thirty seconds and listen for clarity. Discuss the topic with a study partner and correct each other kindly. When reading, underline new vocabulary and write a synonym you already know beside each word.

## Examples
Example 1: Learner A greets Learner B in the morning and asks a simple follow-up question. Example 2: Learner describes one fact using the target vocabulary in a complete sentence. Example 3: Learner listens to a short prompt and selects the best response from two options. Example 4: Learner rewrites a broken sentence into a correct and polite English line. Example 5: Two learners role-play a classroom scenario and swap roles so both speak and listen. Example 6: Learner writes three original sentences, checks them against the lesson notes, and corrects any tense or word-order errors before submitting practice work.

## Summary
You reviewed Essay Writing by studying introduction, objectives, discussion, and examples. Continue practicing aloud for five minutes after each study session. Complete the quiz to check understanding, then revisit any missed questions and read the explanations. Strong learners review little and often rather than cramming once a week. Return to this module when you need a refresher before presentations, interviews, or exams. Share one new sentence with a friend or family member to reinforce memory. Mark the module complete in LexoraED only after you feel comfortable using the language without reading every word from the screen.",
            ModuleCategory.Reading,
            DifficultyLevel.Advanced,
            1,
            null,
            new List<QuizQuestionDefinition>
            {
                new(QuizQuestionType.MultipleChoice, "Which option best matches the main idea of Essay Writing?", "An unrelated science term", "A correct core concept", "A random number only", "A punctuation mark only", "B", "Choose the concept taught in the lesson."),
            new(QuizQuestionType.MultipleChoice, "When practicing Essay Writing, what helps most?", "Memorizing with no context", "Using words in real sentences", "Avoiding all speaking", "Skipping review", "B"),
            new(QuizQuestionType.MultipleChoice, "Which sentence is grammatically appropriate for Essay Writing?", "Me go store yesterday always", "She study careful every day", "They are learning step by step", "Him is go now quick", "C"),
            new(QuizQuestionType.MultipleChoice, "Select the polite response related to Essay Writing.", "Go away now", "Could you repeat that, please?", "I no listen", "That stupid question", "B"),
            new(QuizQuestionType.MultipleChoice, "What should you do after studying Essay Writing?", "Never practice again", "Practice aloud briefly", "Delete your notes", "Ignore the quiz", "B"),
            new(QuizQuestionType.MultipleChoice, "Which study habit supports Essay Writing?", "Cram once a month only", "Review a few minutes often", "Skip all examples", "Avoid asking questions", "B"),
            new(QuizQuestionType.TrueFalse, "True or False: Connecting Essay Writing to daily situations improves retention.", "True", "False", "", "", "True", "Context helps memory."),
            new(QuizQuestionType.TrueFalse, "True or False: You should never speak new words aloud when learning Essay Writing.", "True", "False", "", "", "False", "Speaking supports pronunciation."),
            new(QuizQuestionType.Identification, "Fill in the blank: Good ______ is important when learning Essay Writing.", "", "", "", "", "practice"),
            new(QuizQuestionType.Identification, "Write the missing word: Review your ______ after each lesson on Essay Writing.", "", "", "", "", "notes"),
            new(QuizQuestionType.Situational, "You forget a word during a Essay Writing activity. What is the best action?", "Stay silent forever", "Ask for clarification politely", "Leave the room angrily", "Change the subject rudely", "clarification|repeat|please"),
            new(QuizQuestionType.Situational, "A classmate needs help with Essay Writing. What should you do?", "Mock their mistake", "Offer a short helpful example", "Ignore them completely", "Copy their work", "help|example|support")
            });
        yield return new CurriculumModuleDefinition(
            "Critical Reading",
            "Evaluate arguments, tone, and evidence in texts.",
            @"## Introduction
Welcome to Critical Reading. In this microlesson you will study critical reading through clear explanations, guided practice, and short examples you can use immediately in class or conversation. English becomes easier when you connect new words and patterns to situations you already know, such as meeting classmates, answering the teacher, or describing your day. Read each section carefully and say the sample sentences aloud to build confidence and pronunciation. LexoraED modules are designed as microlessons: each section builds on the previous one so you never feel lost. Take notes in your own language first if that helps, then rewrite key ideas in English. Learning a language is a process, so celebrate small wins such as remembering one new phrase or fixing one grammar mistake during a role-play.

## Learning Objectives
By the end of this lesson you will be able to recognize key words and phrases, use them in simple sentences, answer short questions about the topic, and avoid common beginner mistakes. You will also practice listening for meaning, choosing appropriate responses, and reviewing what you learned with the module quiz. You should be able to explain the topic to a partner in one or two sentences, complete a short written exercise without copying from the screen, and identify at least three expressions that are appropriate in polite conversation. Finally, you will prepare for the twelve-question assessment that checks multiple choice, true or false, identification, and situational judgment skills related to this topic.

## Main Discussion
The main focus of Critical Reading is critical reading. Start with high-frequency expressions learners hear every day. Notice spelling, stress, and polite forms. When you speak, use short sentences: subject plus verb plus complement. Repeat new items in context instead of memorizing isolated lists. Pair words with gestures or pictures when possible. If you are unsure, ask a clarifying question such as Could you repeat that, please? Keep a small notebook for examples you personally need at school, work, or online study. Compare formal and informal registers when the lesson allows: some phrases fit friends, while others fit teachers or supervisors. Watch for false friends from your first language that look like English but mean something different. Record yourself for thirty seconds and listen for clarity. Discuss the topic with a study partner and correct each other kindly. When reading, underline new vocabulary and write a synonym you already know beside each word.

## Examples
Example 1: Learner A greets Learner B in the morning and asks a simple follow-up question. Example 2: Learner describes one fact using the target vocabulary in a complete sentence. Example 3: Learner listens to a short prompt and selects the best response from two options. Example 4: Learner rewrites a broken sentence into a correct and polite English line. Example 5: Two learners role-play a classroom scenario and swap roles so both speak and listen. Example 6: Learner writes three original sentences, checks them against the lesson notes, and corrects any tense or word-order errors before submitting practice work.

## Summary
You reviewed Critical Reading by studying introduction, objectives, discussion, and examples. Continue practicing aloud for five minutes after each study session. Complete the quiz to check understanding, then revisit any missed questions and read the explanations. Strong learners review little and often rather than cramming once a week. Return to this module when you need a refresher before presentations, interviews, or exams. Share one new sentence with a friend or family member to reinforce memory. Mark the module complete in LexoraED only after you feel comfortable using the language without reading every word from the screen.",
            ModuleCategory.Reading,
            DifficultyLevel.Advanced,
            2,
            1,
            new List<QuizQuestionDefinition>
            {
                new(QuizQuestionType.MultipleChoice, "Which option best matches the main idea of Critical Reading?", "An unrelated science term", "A correct core concept", "A random number only", "A punctuation mark only", "B", "Choose the concept taught in the lesson."),
            new(QuizQuestionType.MultipleChoice, "When practicing Critical Reading, what helps most?", "Memorizing with no context", "Using words in real sentences", "Avoiding all speaking", "Skipping review", "B"),
            new(QuizQuestionType.MultipleChoice, "Which sentence is grammatically appropriate for Critical Reading?", "Me go store yesterday always", "She study careful every day", "They are learning step by step", "Him is go now quick", "C"),
            new(QuizQuestionType.MultipleChoice, "Select the polite response related to Critical Reading.", "Go away now", "Could you repeat that, please?", "I no listen", "That stupid question", "B"),
            new(QuizQuestionType.MultipleChoice, "What should you do after studying Critical Reading?", "Never practice again", "Practice aloud briefly", "Delete your notes", "Ignore the quiz", "B"),
            new(QuizQuestionType.MultipleChoice, "Which study habit supports Critical Reading?", "Cram once a month only", "Review a few minutes often", "Skip all examples", "Avoid asking questions", "B"),
            new(QuizQuestionType.TrueFalse, "True or False: Connecting Critical Reading to daily situations improves retention.", "True", "False", "", "", "True", "Context helps memory."),
            new(QuizQuestionType.TrueFalse, "True or False: You should never speak new words aloud when learning Critical Reading.", "True", "False", "", "", "False", "Speaking supports pronunciation."),
            new(QuizQuestionType.Identification, "Fill in the blank: Good ______ is important when learning Critical Reading.", "", "", "", "", "practice"),
            new(QuizQuestionType.Identification, "Write the missing word: Review your ______ after each lesson on Critical Reading.", "", "", "", "", "notes"),
            new(QuizQuestionType.Situational, "You forget a word during a Critical Reading activity. What is the best action?", "Stay silent forever", "Ask for clarification politely", "Leave the room angrily", "Change the subject rudely", "clarification|repeat|please"),
            new(QuizQuestionType.Situational, "A classmate needs help with Critical Reading. What should you do?", "Mock their mistake", "Offer a short helpful example", "Ignore them completely", "Copy their work", "help|example|support")
            });
        yield return new CurriculumModuleDefinition(
            "Grammar Analysis",
            "Analyze complex structures and common error patterns.",
            @"## Introduction
Welcome to Grammar Analysis. In this microlesson you will study grammar analysis through clear explanations, guided practice, and short examples you can use immediately in class or conversation. English becomes easier when you connect new words and patterns to situations you already know, such as meeting classmates, answering the teacher, or describing your day. Read each section carefully and say the sample sentences aloud to build confidence and pronunciation. LexoraED modules are designed as microlessons: each section builds on the previous one so you never feel lost. Take notes in your own language first if that helps, then rewrite key ideas in English. Learning a language is a process, so celebrate small wins such as remembering one new phrase or fixing one grammar mistake during a role-play.

## Learning Objectives
By the end of this lesson you will be able to recognize key words and phrases, use them in simple sentences, answer short questions about the topic, and avoid common beginner mistakes. You will also practice listening for meaning, choosing appropriate responses, and reviewing what you learned with the module quiz. You should be able to explain the topic to a partner in one or two sentences, complete a short written exercise without copying from the screen, and identify at least three expressions that are appropriate in polite conversation. Finally, you will prepare for the twelve-question assessment that checks multiple choice, true or false, identification, and situational judgment skills related to this topic.

## Main Discussion
The main focus of Grammar Analysis is grammar analysis. Start with high-frequency expressions learners hear every day. Notice spelling, stress, and polite forms. When you speak, use short sentences: subject plus verb plus complement. Repeat new items in context instead of memorizing isolated lists. Pair words with gestures or pictures when possible. If you are unsure, ask a clarifying question such as Could you repeat that, please? Keep a small notebook for examples you personally need at school, work, or online study. Compare formal and informal registers when the lesson allows: some phrases fit friends, while others fit teachers or supervisors. Watch for false friends from your first language that look like English but mean something different. Record yourself for thirty seconds and listen for clarity. Discuss the topic with a study partner and correct each other kindly. When reading, underline new vocabulary and write a synonym you already know beside each word.

## Examples
Example 1: Learner A greets Learner B in the morning and asks a simple follow-up question. Example 2: Learner describes one fact using the target vocabulary in a complete sentence. Example 3: Learner listens to a short prompt and selects the best response from two options. Example 4: Learner rewrites a broken sentence into a correct and polite English line. Example 5: Two learners role-play a classroom scenario and swap roles so both speak and listen. Example 6: Learner writes three original sentences, checks them against the lesson notes, and corrects any tense or word-order errors before submitting practice work.

## Summary
You reviewed Grammar Analysis by studying introduction, objectives, discussion, and examples. Continue practicing aloud for five minutes after each study session. Complete the quiz to check understanding, then revisit any missed questions and read the explanations. Strong learners review little and often rather than cramming once a week. Return to this module when you need a refresher before presentations, interviews, or exams. Share one new sentence with a friend or family member to reinforce memory. Mark the module complete in LexoraED only after you feel comfortable using the language without reading every word from the screen.",
            ModuleCategory.Grammar,
            DifficultyLevel.Advanced,
            3,
            2,
            new List<QuizQuestionDefinition>
            {
                new(QuizQuestionType.MultipleChoice, "Which option best matches the main idea of Grammar Analysis?", "An unrelated science term", "A correct core concept", "A random number only", "A punctuation mark only", "B", "Choose the concept taught in the lesson."),
            new(QuizQuestionType.MultipleChoice, "When practicing Grammar Analysis, what helps most?", "Memorizing with no context", "Using words in real sentences", "Avoiding all speaking", "Skipping review", "B"),
            new(QuizQuestionType.MultipleChoice, "Which sentence is grammatically appropriate for Grammar Analysis?", "Me go store yesterday always", "She study careful every day", "They are learning step by step", "Him is go now quick", "C"),
            new(QuizQuestionType.MultipleChoice, "Select the polite response related to Grammar Analysis.", "Go away now", "Could you repeat that, please?", "I no listen", "That stupid question", "B"),
            new(QuizQuestionType.MultipleChoice, "What should you do after studying Grammar Analysis?", "Never practice again", "Practice aloud briefly", "Delete your notes", "Ignore the quiz", "B"),
            new(QuizQuestionType.MultipleChoice, "Which study habit supports Grammar Analysis?", "Cram once a month only", "Review a few minutes often", "Skip all examples", "Avoid asking questions", "B"),
            new(QuizQuestionType.TrueFalse, "True or False: Connecting Grammar Analysis to daily situations improves retention.", "True", "False", "", "", "True", "Context helps memory."),
            new(QuizQuestionType.TrueFalse, "True or False: You should never speak new words aloud when learning Grammar Analysis.", "True", "False", "", "", "False", "Speaking supports pronunciation."),
            new(QuizQuestionType.Identification, "Fill in the blank: Good ______ is important when learning Grammar Analysis.", "", "", "", "", "practice"),
            new(QuizQuestionType.Identification, "Write the missing word: Review your ______ after each lesson on Grammar Analysis.", "", "", "", "", "notes"),
            new(QuizQuestionType.Situational, "You forget a word during a Grammar Analysis activity. What is the best action?", "Stay silent forever", "Ask for clarification politely", "Leave the room angrily", "Change the subject rudely", "clarification|repeat|please"),
            new(QuizQuestionType.Situational, "A classmate needs help with Grammar Analysis. What should you do?", "Mock their mistake", "Offer a short helpful example", "Ignore them completely", "Copy their work", "help|example|support")
            });
        yield return new CurriculumModuleDefinition(
            "Formal Communication",
            "Write and speak professionally in formal settings.",
            @"## Introduction
Welcome to Formal Communication. In this microlesson you will study formal communication through clear explanations, guided practice, and short examples you can use immediately in class or conversation. English becomes easier when you connect new words and patterns to situations you already know, such as meeting classmates, answering the teacher, or describing your day. Read each section carefully and say the sample sentences aloud to build confidence and pronunciation. LexoraED modules are designed as microlessons: each section builds on the previous one so you never feel lost. Take notes in your own language first if that helps, then rewrite key ideas in English. Learning a language is a process, so celebrate small wins such as remembering one new phrase or fixing one grammar mistake during a role-play.

## Learning Objectives
By the end of this lesson you will be able to recognize key words and phrases, use them in simple sentences, answer short questions about the topic, and avoid common beginner mistakes. You will also practice listening for meaning, choosing appropriate responses, and reviewing what you learned with the module quiz. You should be able to explain the topic to a partner in one or two sentences, complete a short written exercise without copying from the screen, and identify at least three expressions that are appropriate in polite conversation. Finally, you will prepare for the twelve-question assessment that checks multiple choice, true or false, identification, and situational judgment skills related to this topic.

## Main Discussion
The main focus of Formal Communication is formal communication. Start with high-frequency expressions learners hear every day. Notice spelling, stress, and polite forms. When you speak, use short sentences: subject plus verb plus complement. Repeat new items in context instead of memorizing isolated lists. Pair words with gestures or pictures when possible. If you are unsure, ask a clarifying question such as Could you repeat that, please? Keep a small notebook for examples you personally need at school, work, or online study. Compare formal and informal registers when the lesson allows: some phrases fit friends, while others fit teachers or supervisors. Watch for false friends from your first language that look like English but mean something different. Record yourself for thirty seconds and listen for clarity. Discuss the topic with a study partner and correct each other kindly. When reading, underline new vocabulary and write a synonym you already know beside each word.

## Examples
Example 1: Learner A greets Learner B in the morning and asks a simple follow-up question. Example 2: Learner describes one fact using the target vocabulary in a complete sentence. Example 3: Learner listens to a short prompt and selects the best response from two options. Example 4: Learner rewrites a broken sentence into a correct and polite English line. Example 5: Two learners role-play a classroom scenario and swap roles so both speak and listen. Example 6: Learner writes three original sentences, checks them against the lesson notes, and corrects any tense or word-order errors before submitting practice work.

## Summary
You reviewed Formal Communication by studying introduction, objectives, discussion, and examples. Continue practicing aloud for five minutes after each study session. Complete the quiz to check understanding, then revisit any missed questions and read the explanations. Strong learners review little and often rather than cramming once a week. Return to this module when you need a refresher before presentations, interviews, or exams. Share one new sentence with a friend or family member to reinforce memory. Mark the module complete in LexoraED only after you feel comfortable using the language without reading every word from the screen.",
            ModuleCategory.Reading,
            DifficultyLevel.Advanced,
            4,
            3,
            new List<QuizQuestionDefinition>
            {
                new(QuizQuestionType.MultipleChoice, "Which option best matches the main idea of Formal Communication?", "An unrelated science term", "A correct core concept", "A random number only", "A punctuation mark only", "B", "Choose the concept taught in the lesson."),
            new(QuizQuestionType.MultipleChoice, "When practicing Formal Communication, what helps most?", "Memorizing with no context", "Using words in real sentences", "Avoiding all speaking", "Skipping review", "B"),
            new(QuizQuestionType.MultipleChoice, "Which sentence is grammatically appropriate for Formal Communication?", "Me go store yesterday always", "She study careful every day", "They are learning step by step", "Him is go now quick", "C"),
            new(QuizQuestionType.MultipleChoice, "Select the polite response related to Formal Communication.", "Go away now", "Could you repeat that, please?", "I no listen", "That stupid question", "B"),
            new(QuizQuestionType.MultipleChoice, "What should you do after studying Formal Communication?", "Never practice again", "Practice aloud briefly", "Delete your notes", "Ignore the quiz", "B"),
            new(QuizQuestionType.MultipleChoice, "Which study habit supports Formal Communication?", "Cram once a month only", "Review a few minutes often", "Skip all examples", "Avoid asking questions", "B"),
            new(QuizQuestionType.TrueFalse, "True or False: Connecting Formal Communication to daily situations improves retention.", "True", "False", "", "", "True", "Context helps memory."),
            new(QuizQuestionType.TrueFalse, "True or False: You should never speak new words aloud when learning Formal Communication.", "True", "False", "", "", "False", "Speaking supports pronunciation."),
            new(QuizQuestionType.Identification, "Fill in the blank: Good ______ is important when learning Formal Communication.", "", "", "", "", "practice"),
            new(QuizQuestionType.Identification, "Write the missing word: Review your ______ after each lesson on Formal Communication.", "", "", "", "", "notes"),
            new(QuizQuestionType.Situational, "You forget a word during a Formal Communication activity. What is the best action?", "Stay silent forever", "Ask for clarification politely", "Leave the room angrily", "Change the subject rudely", "clarification|repeat|please"),
            new(QuizQuestionType.Situational, "A classmate needs help with Formal Communication. What should you do?", "Mock their mistake", "Offer a short helpful example", "Ignore them completely", "Copy their work", "help|example|support")
            });
        yield return new CurriculumModuleDefinition(
            "Public Speaking English",
            "Deliver organized speeches with clarity and confidence.",
            @"## Introduction
Welcome to Public Speaking English. In this microlesson you will study public speaking english through clear explanations, guided practice, and short examples you can use immediately in class or conversation. English becomes easier when you connect new words and patterns to situations you already know, such as meeting classmates, answering the teacher, or describing your day. Read each section carefully and say the sample sentences aloud to build confidence and pronunciation. LexoraED modules are designed as microlessons: each section builds on the previous one so you never feel lost. Take notes in your own language first if that helps, then rewrite key ideas in English. Learning a language is a process, so celebrate small wins such as remembering one new phrase or fixing one grammar mistake during a role-play.

## Learning Objectives
By the end of this lesson you will be able to recognize key words and phrases, use them in simple sentences, answer short questions about the topic, and avoid common beginner mistakes. You will also practice listening for meaning, choosing appropriate responses, and reviewing what you learned with the module quiz. You should be able to explain the topic to a partner in one or two sentences, complete a short written exercise without copying from the screen, and identify at least three expressions that are appropriate in polite conversation. Finally, you will prepare for the twelve-question assessment that checks multiple choice, true or false, identification, and situational judgment skills related to this topic.

## Main Discussion
The main focus of Public Speaking English is public speaking english. Start with high-frequency expressions learners hear every day. Notice spelling, stress, and polite forms. When you speak, use short sentences: subject plus verb plus complement. Repeat new items in context instead of memorizing isolated lists. Pair words with gestures or pictures when possible. If you are unsure, ask a clarifying question such as Could you repeat that, please? Keep a small notebook for examples you personally need at school, work, or online study. Compare formal and informal registers when the lesson allows: some phrases fit friends, while others fit teachers or supervisors. Watch for false friends from your first language that look like English but mean something different. Record yourself for thirty seconds and listen for clarity. Discuss the topic with a study partner and correct each other kindly. When reading, underline new vocabulary and write a synonym you already know beside each word.

## Examples
Example 1: Learner A greets Learner B in the morning and asks a simple follow-up question. Example 2: Learner describes one fact using the target vocabulary in a complete sentence. Example 3: Learner listens to a short prompt and selects the best response from two options. Example 4: Learner rewrites a broken sentence into a correct and polite English line. Example 5: Two learners role-play a classroom scenario and swap roles so both speak and listen. Example 6: Learner writes three original sentences, checks them against the lesson notes, and corrects any tense or word-order errors before submitting practice work.

## Summary
You reviewed Public Speaking English by studying introduction, objectives, discussion, and examples. Continue practicing aloud for five minutes after each study session. Complete the quiz to check understanding, then revisit any missed questions and read the explanations. Strong learners review little and often rather than cramming once a week. Return to this module when you need a refresher before presentations, interviews, or exams. Share one new sentence with a friend or family member to reinforce memory. Mark the module complete in LexoraED only after you feel comfortable using the language without reading every word from the screen.",
            ModuleCategory.Reading,
            DifficultyLevel.Advanced,
            5,
            4,
            new List<QuizQuestionDefinition>
            {
                new(QuizQuestionType.MultipleChoice, "Which option best matches the main idea of Public Speaking English?", "An unrelated science term", "A correct core concept", "A random number only", "A punctuation mark only", "B", "Choose the concept taught in the lesson."),
            new(QuizQuestionType.MultipleChoice, "When practicing Public Speaking English, what helps most?", "Memorizing with no context", "Using words in real sentences", "Avoiding all speaking", "Skipping review", "B"),
            new(QuizQuestionType.MultipleChoice, "Which sentence is grammatically appropriate for Public Speaking English?", "Me go store yesterday always", "She study careful every day", "They are learning step by step", "Him is go now quick", "C"),
            new(QuizQuestionType.MultipleChoice, "Select the polite response related to Public Speaking English.", "Go away now", "Could you repeat that, please?", "I no listen", "That stupid question", "B"),
            new(QuizQuestionType.MultipleChoice, "What should you do after studying Public Speaking English?", "Never practice again", "Practice aloud briefly", "Delete your notes", "Ignore the quiz", "B"),
            new(QuizQuestionType.MultipleChoice, "Which study habit supports Public Speaking English?", "Cram once a month only", "Review a few minutes often", "Skip all examples", "Avoid asking questions", "B"),
            new(QuizQuestionType.TrueFalse, "True or False: Connecting Public Speaking English to daily situations improves retention.", "True", "False", "", "", "True", "Context helps memory."),
            new(QuizQuestionType.TrueFalse, "True or False: You should never speak new words aloud when learning Public Speaking English.", "True", "False", "", "", "False", "Speaking supports pronunciation."),
            new(QuizQuestionType.Identification, "Fill in the blank: Good ______ is important when learning Public Speaking English.", "", "", "", "", "practice"),
            new(QuizQuestionType.Identification, "Write the missing word: Review your ______ after each lesson on Public Speaking English.", "", "", "", "", "notes"),
            new(QuizQuestionType.Situational, "You forget a word during a Public Speaking English activity. What is the best action?", "Stay silent forever", "Ask for clarification politely", "Leave the room angrily", "Change the subject rudely", "clarification|repeat|please"),
            new(QuizQuestionType.Situational, "A classmate needs help with Public Speaking English. What should you do?", "Mock their mistake", "Offer a short helpful example", "Ignore them completely", "Copy their work", "help|example|support")
            });
        yield return new CurriculumModuleDefinition(
            "Advanced Vocabulary",
            "Use precise academic and professional word choices.",
            @"## Introduction
Welcome to Advanced Vocabulary. In this microlesson you will study advanced vocabulary through clear explanations, guided practice, and short examples you can use immediately in class or conversation. English becomes easier when you connect new words and patterns to situations you already know, such as meeting classmates, answering the teacher, or describing your day. Read each section carefully and say the sample sentences aloud to build confidence and pronunciation. LexoraED modules are designed as microlessons: each section builds on the previous one so you never feel lost. Take notes in your own language first if that helps, then rewrite key ideas in English. Learning a language is a process, so celebrate small wins such as remembering one new phrase or fixing one grammar mistake during a role-play.

## Learning Objectives
By the end of this lesson you will be able to recognize key words and phrases, use them in simple sentences, answer short questions about the topic, and avoid common beginner mistakes. You will also practice listening for meaning, choosing appropriate responses, and reviewing what you learned with the module quiz. You should be able to explain the topic to a partner in one or two sentences, complete a short written exercise without copying from the screen, and identify at least three expressions that are appropriate in polite conversation. Finally, you will prepare for the twelve-question assessment that checks multiple choice, true or false, identification, and situational judgment skills related to this topic.

## Main Discussion
The main focus of Advanced Vocabulary is advanced vocabulary. Start with high-frequency expressions learners hear every day. Notice spelling, stress, and polite forms. When you speak, use short sentences: subject plus verb plus complement. Repeat new items in context instead of memorizing isolated lists. Pair words with gestures or pictures when possible. If you are unsure, ask a clarifying question such as Could you repeat that, please? Keep a small notebook for examples you personally need at school, work, or online study. Compare formal and informal registers when the lesson allows: some phrases fit friends, while others fit teachers or supervisors. Watch for false friends from your first language that look like English but mean something different. Record yourself for thirty seconds and listen for clarity. Discuss the topic with a study partner and correct each other kindly. When reading, underline new vocabulary and write a synonym you already know beside each word.

## Examples
Example 1: Learner A greets Learner B in the morning and asks a simple follow-up question. Example 2: Learner describes one fact using the target vocabulary in a complete sentence. Example 3: Learner listens to a short prompt and selects the best response from two options. Example 4: Learner rewrites a broken sentence into a correct and polite English line. Example 5: Two learners role-play a classroom scenario and swap roles so both speak and listen. Example 6: Learner writes three original sentences, checks them against the lesson notes, and corrects any tense or word-order errors before submitting practice work.

## Summary
You reviewed Advanced Vocabulary by studying introduction, objectives, discussion, and examples. Continue practicing aloud for five minutes after each study session. Complete the quiz to check understanding, then revisit any missed questions and read the explanations. Strong learners review little and often rather than cramming once a week. Return to this module when you need a refresher before presentations, interviews, or exams. Share one new sentence with a friend or family member to reinforce memory. Mark the module complete in LexoraED only after you feel comfortable using the language without reading every word from the screen.",
            ModuleCategory.Vocabulary,
            DifficultyLevel.Advanced,
            6,
            5,
            new List<QuizQuestionDefinition>
            {
                new(QuizQuestionType.MultipleChoice, "Which option best matches the main idea of Advanced Vocabulary?", "An unrelated science term", "A correct core concept", "A random number only", "A punctuation mark only", "B", "Choose the concept taught in the lesson."),
            new(QuizQuestionType.MultipleChoice, "When practicing Advanced Vocabulary, what helps most?", "Memorizing with no context", "Using words in real sentences", "Avoiding all speaking", "Skipping review", "B"),
            new(QuizQuestionType.MultipleChoice, "Which sentence is grammatically appropriate for Advanced Vocabulary?", "Me go store yesterday always", "She study careful every day", "They are learning step by step", "Him is go now quick", "C"),
            new(QuizQuestionType.MultipleChoice, "Select the polite response related to Advanced Vocabulary.", "Go away now", "Could you repeat that, please?", "I no listen", "That stupid question", "B"),
            new(QuizQuestionType.MultipleChoice, "What should you do after studying Advanced Vocabulary?", "Never practice again", "Practice aloud briefly", "Delete your notes", "Ignore the quiz", "B"),
            new(QuizQuestionType.MultipleChoice, "Which study habit supports Advanced Vocabulary?", "Cram once a month only", "Review a few minutes often", "Skip all examples", "Avoid asking questions", "B"),
            new(QuizQuestionType.TrueFalse, "True or False: Connecting Advanced Vocabulary to daily situations improves retention.", "True", "False", "", "", "True", "Context helps memory."),
            new(QuizQuestionType.TrueFalse, "True or False: You should never speak new words aloud when learning Advanced Vocabulary.", "True", "False", "", "", "False", "Speaking supports pronunciation."),
            new(QuizQuestionType.Identification, "Fill in the blank: Good ______ is important when learning Advanced Vocabulary.", "", "", "", "", "practice"),
            new(QuizQuestionType.Identification, "Write the missing word: Review your ______ after each lesson on Advanced Vocabulary.", "", "", "", "", "notes"),
            new(QuizQuestionType.Situational, "You forget a word during a Advanced Vocabulary activity. What is the best action?", "Stay silent forever", "Ask for clarification politely", "Leave the room angrily", "Change the subject rudely", "clarification|repeat|please"),
            new(QuizQuestionType.Situational, "A classmate needs help with Advanced Vocabulary. What should you do?", "Mock their mistake", "Offer a short helpful example", "Ignore them completely", "Copy their work", "help|example|support")
            });    }
}
