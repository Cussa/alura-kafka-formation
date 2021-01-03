# Kafka Formation - Alura Cursos

Portuguese version bellow.

This is a C# implementation for the [Kafka Formation](https://cursos.alura.com.br/formacao-kafka) from the [@alura-cursosname](https://github.com/alura-cursos)

![alt text](https://github.com/Cussa/alura-kafka-formation/blob/main/images/ServiceRunning.png?raw=true)

## Courses covered until now
- [Kafka: Produtores, Consumidores e streams](https://cursos.alura.com.br/course/kafka-introducao-a-streams-em-microservicos)

## How to run

- Install Visual Studio and Docker on your machine
- Download the project
- Run the `ecommerce-start.ps1` on a powershell command. It automatically builds the project and give you some options:
	- Start an independent consumer for the LogService, EmailService and FraudDetectorService
	- Start or stop the docker compose
	- Generate 10 random orders for the test

Ports used: 9092 (Kafka) and 2181 (Zookeeper).

In case you want to change the number of partitions for some topic, you can do that directly on the docker-compose.yml file, where the command follow the following pattern: `<topic name>:<number of partitions>:<number of replicas>`. So, if you want to do something like is done on the course, you can change the configuration to:
```
KAFKA_CREATE_TOPICS: "ECOMMERCE_NEW_ORDER:3:1,ECOMMERCE_SEND_EMAIL:1:1"
```

## Differences comparing with the Original from [@alura-cursosname](https://github.com/alura-cursos)

As the original is done in Java, there are some differences between their code and mine:
- On the DotNet library, you don't need to specify the key and value serializer/deserializer always. By default, it already uses a String Serializer/Deserializer. And that works a little bit different on the library. You have to create a builder and pass the de/serializer to it.
```
var builder = new ConsumerBuilder<string, T>(GetProperties(groupId, properties));
if (deserializer != null)
    builder.SetValueDeserializer(deserializer);
_consumer = builder.Build();
```
- I created a function that verifies if there is some change on the partitions that the consumer is assigned. If yes, it print some information about the new partition. Every time it consumes a message, it will verify if there is a change. This is a validation that should only be done in debug options and should not be used in production environment.
- In the KafkaDispatcher and KafkaConsumer, there are some commented configuration about the logs from Kafka Library. Uncomment it if you want to see the Kafka logs on the console.
- As far as I got, there is no way to follow some of the configuration that is done in the course. However, everything seems to be working without problems.
- To avoid mistypes, I create a Topic class that have the topics names in constants and everywhere that need the topics, uses this class.

# Portuguese Version

# Formação Kafka - Alura Cursos

Esta é uma implementação em C# da [Formação Kafka](https://cursos.alura.com.br/formacao-kafka) da [@alura-cursosname](https://github.com/alura-cursos)

## Cursos abordados até o momento
- [Kafka: Produtores, Consumidores e streams](https://cursos.alura.com.br/course/kafka-introducao-a-streams-em-microservicos)

## Como rodar o projeto

- Instale o Visual Studio e o Docker na sua máquina
- Faça o download do projeto
- Execute o comando `ecommerce-start.ps1` no PowerShell. Ele irá efetuar o build do projeto e irá lhe dar algumas opções:
	- Iniciar consumidores independentes para o LogService, EmailService e FraudDetectorService
	- Iniciar ou parar o docker compose
	- Gerar 10 ordens randomicas para teste

Portas usadas: 9092 (Kafka) and 2181 (Zookeeper).

Caso você queira mudar o número de partições para algum tópico, você pode fazer isso direto no arquio docker-compose.yml, onde o comando segue o seguinte padrão: `<nome do tópico>:<número de partições>:<número de réplicas>`. Logo, se você quiser fazer algo como feito durante o curso, você pode mudar a configuração para:
```
KAFKA_CREATE_TOPICS: "ECOMMERCE_NEW_ORDER:3:1,ECOMMERCE_SEND_EMAIL:1:1"
```

## Diferenças em comparação a versão original da [@alura-cursosname](https://github.com/alura-cursos)

Como a versão original foi feita em Java, há algumas diferenças entre o código deles e o meu:
- Na biblioteca do Kafka-DotNet, você não precisa especificar sempre um serializador/deserializador. Por padrão, ele irá sempre usar um de/serializador para Strings. E isso funciona um pouco diferente na biblioteca. Você precisar criar um builder e passar o serializador para ele:
```
var builder = new ConsumerBuilder<string, T>(GetProperties(groupId, properties));
if (deserializer != null)
    builder.SetValueDeserializer(deserializer);
_consumer = builder.Build();
```
- Eu criei uma função que verifica se houve alguma mudança na lista de partições que está configurada para o consumidor. Caso tenha, ele irá imprimir as informações sobre a nova partição. Toda vez que ele consome uma mensagem, ele verifica se há alteração. Essa validação deve ser feita apenas em dbug e não deve ser usada em ambientes de produção.
- No KafkaDispatcher e KafkaConsumer, eu deixei algumas linhdas de configuração de logs comentadas, que se conectam direto na biblioteca do Kafka. Descomente essas linhas caso você queira ver os logs do Kafka no console.
- Até onde eu entendi, algumas configurações presentes na biblioteca do Java não estão disponíveis na versão DotNet. Porém, tudo parece estar funcionando sem problemas.
- Para evitar erros de digitação, eu criei uma class para os tópicos `Topics` e todo lugar que precisa usar o nome do tópico, utiliza essa classe.