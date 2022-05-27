import 'package:flutter/material.dart';
import 'package:provider/provider.dart';
import 'package:t9n_manager_flutter_client/domains/project/apis/api_project.dart';
import 'package:t9n_manager_flutter_client/domains/project/widgets/project_card.dart';
import 'package:t9n_manager_flutter_client/shared/alert.dart';
import 'package:t9n_manager_flutter_client/shared/app_state_notifier.dart';
import '../../../generated/l10n.dart';
import '../../../shared/app_settings.dart';
import '../../../shared/models/api_exception.dart';
import '../models/project.dart';
import '../widgets/project_card.dart';

class ProjectScreen extends StatefulWidget {
  const ProjectScreen({Key? key}) : super(key: key);

  @override
  State<ProjectScreen> createState() => _ProjectScreenState();
}

class _ProjectScreenState extends State<ProjectScreen> {
  bool toBeRefreshed = false;
  @override
  void initState() {
    toBeRefreshed = false;
    super.initState();
  }

  @override
  Widget build(BuildContext context) {
    AppSettings appSettings = context.watch<AppSettings>();
    AppState appState = context.watch<AppState>();
    var jwt = appState.jwt;
    var currentTenant = appState.selectedTenant;
    double width = MediaQuery.of(context).size.width > context.watch<AppState>().screenMaxSize ? context.watch<AppState>().screenMaxSize : MediaQuery.of(context).size.width;
    double cardSize = context.watch<AppState>().cardSize;
    int crossAxisCount = (width / cardSize).floor();

    return FutureBuilder<List<Project>>(
        future: getAllProjects(appSettings, jwt, context, currentTenant),
        builder: (context, AsyncSnapshot<List<Project>> snapshot) {
          if (snapshot.connectionState != ConnectionState.done) {
            return const CircularProgressIndicator();
          } else if (snapshot.hasData) {
            return Scaffold(
              appBar: AppBar(
                title: Text(S.of(context).title),
              ),
              body: getProjectCards(snapshot.requireData, crossAxisCount),
            );
          } else if (snapshot.hasError) {
            ApiException e = snapshot.error as ApiException;
            if (e.code == 401) {
              return alert(S.of(context).api_error_title, e.t9nMessage, S.of(context).common_button_ok, "/relog", context);
            } else {
              return alert(S.of(context).api_error_title, e.t9nMessage, S.of(context).common_button_ok, "", context);
            }
          } else {
            return const Text("");
          }
        });
  }

  Widget getProjectCards(List<Project> projects, int crossAxisCount) {
    try {
      List<Widget> listProjectCards = <Widget>[];
      for (var counter = 0; counter < projects.length; counter++) {
        listProjectCards.add(ProjectCard(project: projects[counter], addNew: false, refresh: refresh));
      }
      listProjectCards.add(ProjectCard(project: Project("_", S.of(context).project_add_button), addNew: true, refresh: refresh));
      return GridView.count(
        crossAxisCount: crossAxisCount,
        padding: const EdgeInsets.all(5.0),
        children: listProjectCards,
      );
    } catch (e) {
      rethrow;
    }
  }

  refresh() {
    setState(() {
      toBeRefreshed = !toBeRefreshed;
    });
  }
}
