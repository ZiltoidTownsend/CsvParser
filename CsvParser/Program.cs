using CsvParser;

List<TestDto> testDtoList;

using (FileStream fs = new FileStream("log.csv", FileMode.Open))
{
    Parser csvParser = new Parser();

    testDtoList = csvParser.ParseToDto<TestDto>(fs);

}

List<Case> cases = new List<Case>();

List<Message> messages = new List<Message>();

foreach (var dto in testDtoList)
{
    if (dto.MessageId != "73f57721-4314-4c56-a165-167df53e9788")
    {
        messages.Add(new Message { Id = Guid.Parse(dto.MessageId) });

        Console.WriteLine($"В справочник сообщений добавлена запись с Id {dto.MessageId}");

        var @case = new Case
        {
            Recipient = dto.RecipientAddress,
            Sender = dto.SenderAddress,
            Subject = dto.MessageSubject
        };

        cases.Add(@case);

        Console.WriteLine($"В раздел Обращения добавлена запись с полями: " +
            $"{nameof(@case.Recipient)}:{@case.Recipient}, { nameof(@case.Sender)}:{ @case.Sender}, { nameof(@case.Subject)}:{ @case.Subject},");
    }
}



class TestDto
{
    [Index(2)]
    public string MessageId { get; set; }
    [Index(4)]
    public string RecipientAddress { get; set; }
    [Index(1)]
    public string SenderAddress { get; set; }
    [Index(6)]
    public string MessageSubject { get; set; }
}
