##Instructions
Instructions to test 

Para la resolucion se utilizó:
-BACKEND -> .net core 3.1
-FRONTEND -> Angular 9.1.13
-PERSISTENCIA -> Sql server 2017 Developer Edition

Al iniciar la solucion, hacer un restore package.
Elegir Challenge.API como proyecto de inicio.

Para correr el proyecto CHallenge.Frontend es necesario  en necesario hacer un "npm install" para instalar dependencias.
Para ello, situarse en la carpeta web, click derecho y hacer un copy, esto nos copia el path de la carpeta. Accedemos a ella desde CMD y corremos el comando anteriormente mencionado(siempre desde /web).
Otra alternatica es boton derecho sobre web, "open folder in file explorer" que nos abre el explorador de archivos, y sobre la ruta que nos muesta el explorador ponemos "CMD o powershell, lo que usemos", esto nos abre la consola en la ruta especificada ;).
Para levantar el front hecho en angular, hay que correr el comando "ng serve". 
El frontend si bien es solo a modo demostrativo, contempla el tipo de rol y en base a eso muestra un boton create en la parte de meetings. Tiene un funcionamiento basico, maneja jwt y hace uso de servicios rest para peticionar al backend.

Para Sqlserver se subieron varios archivos, se recomienda generar una base de datos llamada "Meetup" y correr el script "CreationWithData.sql" que tiene datos insertados. Asimismo hay un backup de la base de datos en un archivo.bkp que s e puede utilzar para restaurar la base.

Algunas funcionalidades no se hicieron, sobre todo algunos crud de city y user por motivos de tiempo.
Dejé muchos comentarios(abusé un poco) de forma que se pueda entender lo que pensé en ese momento del desarrollo.
En cuanto a la api del clima, solo te calcula hasta 2 dias antes de la fecha, y calcula en base a la hora de la meeting para el dia indicado.

Se trato de hacer una app lo mas desacoplada posible.
Como mejoras le falta autenticacion mas real, logging, cache, healthcheck, hateoas. 


DATOS DE PRUEBA para los usuarios.
usuarios "admin" 
	matsuoluciano@gmail.com 	luciano1
	admin1@gmail.com		admin123

usuarios "user"
	user0@gmail.com			user1234
	--van del 0 al 6--
	user6@gmail.com			user1234

***IMPORTANTE***
En caso de modificar desde el backend la app url(usa SSL), hay que modificar el archivo environment.ts(propiedad restServiceUrl) para que los servicios que utiliza angular, puedan pegarle al backend.
