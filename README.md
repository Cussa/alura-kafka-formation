# Kafka Formation - Alura Cursos

[Clique aqui para ver a versão em Português.](#forma%C3%A7%C3%A3o-kafka---alura-cursos)

This is a C# implementation for the [Kafka Formation](https://cursos.alura.com.br/formacao-kafka) from the [@alura-cursos](https://github.com/alura-cursos)

![alt text](https://github.com/Cussa/alura-kafka-formation/blob/main/images/ServiceRunning.png?raw=true)

## Courses covered until now
- [Kafka: Produtores, Consumidores e streams](https://cursos.alura.com.br/course/kafka-introducao-a-streams-em-microservicos)
- [Kafka: Fast delegate, evolução e cluster de brokers](https://cursos.alura.com.br/course/kafka-cluster-de-microservicos)

## How to run

- Install Visual Studio and Docker on your machine
- Download the project
- Run the [`ecommerce-start.ps1`](ecommerce-start.ps1) on a powershell command. It automatically builds the project and give you some options:
	- Start an independent consumer for the LogService, EmailService and FraudDetectorService
	- Start or stop the docker compose
	- Generate 10 random orders for the test

Ports used: 9092 (Kafka) and 2181 (Zookeeper).

WebSite: 8081 (I had some problems running on 8080 as I have another services running on this port).

In case you want to change the number of partitions for some topic, you can do that directly on the docker-compose.yml file, where the command follow the following pattern: `<topic name>:<number of partitions>:<number of replicas>`. So, if you want to do something like is done on the course, you can change the configuration to:
```
KAFKA_CREATE_TOPICS: "ECOMMERCE_NEW_ORDER:3:1,ECOMMERCE_SEND_EMAIL:1:1"
```

## Differences comparing with the Original from [@alura-cursos](https://github.com/alura-cursos)

As the original is done in Java, there are some differences between their code and mine:
- On the DotNet library, you don't need to specify the key and value serializer/deserializer always. By default, it already uses a String Serializer/Deserializer. And that works a little bit different on the library. You have to create a builder and pass the de/serializer to it.
```
var builder = new ConsumerBuilder<string, T>(GetProperties(groupId, properties));
if (deserializer != null)
    builder.SetValueDeserializer(deserializer);
_consumer = builder.Build();
```
- In the KafkaDispatcher and KafkaConsumer, there are some commented configuration about the logs from Kafka Library. Uncomment it if you want to see the Kafka logs on the console.
- As far as I got, there is no way to follow some of the configuration that is done in the course. However, everything seems to be working without problems.
- To avoid mistypes, I create a Topic class that have the topics names in constants and everywhere that need the topics, uses this class.
- In the course, the intructor created a GetAmount() method, as it's considered the better pattern to follow in Java. However, the same doesn't happen in C#. So, I decided to use the public property without the *set*, which means you can access the value but can't modified it.
- Instead of override the ToString writing the properties from the Order class, I generated a extension method that serialize the object to a json string.
- Instead of using the ADO connections directly to the SQLite, I used the Entity Framework Core. When we run the User Service for the first time, it automatically create the database and run the migrations.

#### Kafka: Fast delegate, evolução e cluster de brokers - Class 4

In case you want to follow along with the instructor, the properties file is inside the following folder: `/opt/kafka_2.13-2.6/config`.
One thing that you will need to do is to comment the final configuration on the `server2.properties`, as the default run will already configure the group port and address:
```
#advertised.port=9094
#advertised.host.name=192.168.1.79
#port=9092
```
And then you have to open the port that you will use the second kafka manually. So, on the [`docker-compose.yml`](docker-compose.yml), configure the port "9093:9093" too.
However, as the docker image starts the Kafka automatically, you will face some issues to do the replications change, as showed on the course.

To make it a little bit easier, we can start some other containers and scale Kafka from Docker. To do that, you should first modify the file [`docker-compose-cluster.yml`](docker-compose-cluster.yml), changing the `KAFKA_ADVERTISED_HOST_NAME` to your local ip address. Then, you can run the following command: `docker-compose -f .\docker-compose-cluster.yml up --scale kafka=2 -d`. It will start to services for Kafka, with the replication set to 2 already. Changing this file, you can start even more containers running the Kafka with the replication.

### Kafka: Batches, correlation ids e dead letters - Class 1

- In the original, the instructor just create a new "Main" method for the batch processor. This is possible because you can run directly the file from the IDE. However, in C# that is not possible. So, to reproduce a similar behavior, I changed the [Main method](src\Ecommerce.Service.Users\UserService.cs) for the UserService and define that when it runs, it asks the which service it should run. In case you want both (New user creation and Batch service), you can run the service two times, one for each service. On the PS Script, you have the shortcuts for both of them.

 ### Kafka: Batches, correlation ids e dead letters - Class 2

- As the Kafka DotNet library has a class called Message, I called my class [`KafkaMessage`](src\Ecommerce.Common\KafkaDispatcher.cs).
- The Json deserialization is able to use the generics to do the work without needing the `type`, as done in the course. However, it has some issues to deserialize a json object to string directly. So, I created a converter on the JsonKafkaAdapter.
- With the changes made, the constructor for the `KafkaService` is simpler now. You only need to specify the `IDeserializer` in case you want to work with a different type comparing with the `T` type.

# Formação Kafka - Alura Cursos

Esta é uma implementação em C# da [Formação Kafka](https://cursos.alura.com.br/formacao-kafka) da [@alura-cursos](https://github.com/alura-cursos)

## Cursos abordados até o momento
- [Kafka: Produtores, Consumidores e streams](https://cursos.alura.com.br/course/kafka-introducao-a-streams-em-microservicos)
- [Kafka: Fast delegate, evolução e cluster de brokers](https://cursos.alura.com.br/course/kafka-cluster-de-microservicos)

## Como rodar o projeto

- Instale o Visual Studio e o Docker na sua máquina
- Faça o download do projeto
- Execute o comando [`ecommerce-start.ps1`](ecommerce-start.ps1) no PowerShell. Ele irá efetuar o build do projeto e irá lhe dar algumas opções:
	- Iniciar consumidores independentes para o LogService, EmailService e FraudDetectorService
	- Iniciar ou parar o docker compose
	- Gerar 10 ordens randomicas para teste

Portas usadas: 9092 (Kafka) and 2181 (Zookeeper).

WebSite: 8081 (Eu tive alguns problemas para rodar na porta 8080 já que tenho outro serviço rodando nesta porta).

Caso você queira mudar o número de partições para algum tópico, você pode fazer isso direto no arquio docker-compose.yml, onde o comando segue o seguinte padrão: `<nome do tópico>:<número de partições>:<número de réplicas>`. Logo, se você quiser fazer algo como feito durante o curso, você pode mudar a configuração para:
```
KAFKA_CREATE_TOPICS: "ECOMMERCE_NEW_ORDER:3:1,ECOMMERCE_SEND_EMAIL:1:1"
```

## Diferenças em comparação a versão original da [@alura-cursos](https://github.com/alura-cursos)

Como a versão original foi feita em Java, há algumas diferenças entre o código deles e o meu:
- Na biblioteca do Kafka-DotNet, você não precisa especificar sempre um serializador/deserializador. Por padrão, ele irá sempre usar um de/serializador para Strings. E isso funciona um pouco diferente na biblioteca. Você precisar criar um builder e passar o serializador para ele:
```
var builder = new ConsumerBuilder<string, T>(GetProperties(groupId, properties));
if (deserializer != null)
    builder.SetValueDeserializer(deserializer);
_consumer = builder.Build();
```
- No KafkaDispatcher e KafkaConsumer, eu deixei algumas linhdas de configuração de logs comentadas, que se conectam direto na biblioteca do Kafka. Descomente essas linhas caso você queira ver os logs do Kafka no console.
- Até onde eu entendi, algumas configurações presentes na biblioteca do Java não estão disponíveis na versão DotNet. Porém, tudo parece estar funcionando sem problemas.
- Para evitar erros de digitação, eu criei uma class para os tópicos `Topics` e todo lugar que precisa usar o nome do tópico, utiliza essa classe.
- No curso, é utilizado o padrão de criar um método GetAmount(), com a justificativa de boa prática em termos de Java. Já em C#, isso não ocorre. Por isso, optei por manter a propriedade como pública, mas sem o *set* público. Logo, ela é como uma propriedade final.
- Ao invés de sobescrever o método ToString usando as propriedades da classe Order, eu criei um método de extensão que serializa o objeto para uma string com formato Json.
- Ao invés de usar conexões com ADO direto para o SQLite, eu optei por usar o Entity Framework Core. Ao abrir o sistema de Usuários, ele já cria o banco e executa a Migration.

#### Kafka: Fast delegate, evolução e cluster de brokers - Aula 4

Caso você queira seguir o professor, o arquivo de propriedades está dentro da pasta: `/opt/kafka_2.13-2.6/config`.
Você irá precisar comentar algumas linhas no final do arquivo de configurações, já que o default irá fazer a configuração de porta e endereço do grupo:
```
#advertised.port=9094
#advertised.host.name=192.168.1.79
#port=9092
```
E você irá precisar abrir uma segnda porta para o kafka manualmente. Logo, no arquivo [`docker-compose.yml`](docker-compose.yml), configure a porta "9093:9093" também.
Porém, já que o docker irá iniciar o Kafka automaticamente, você poderá ter alguns problemas em termos de troca de replicações, como mostrado no curso.

Para tornar a tarefa um pouco mais simples, nós podemos iniciar mais containers e escalar o Kafka direto no Docker. Para fazer isso, você primeiro deve alterar o arquivo [`docker-compose-cluster.yml`](docker-compose-cluster.yml), modificando o `KAFKA_ADVERTISED_HOST_NAME` para o seu ip local. Depois, você pode rodar o seguinte comando: `docker-compose -f .\docker-compose-cluster.yml up --scale kafka=2 -d`. Ele irá iniciar o serviço do Docker com 2 containers para o Kafka, rodando em sistemas de réplicas. Com simples alterações no `docker-compose-cluster.yml`, você pode aumentar o número de réplicas rodando.
