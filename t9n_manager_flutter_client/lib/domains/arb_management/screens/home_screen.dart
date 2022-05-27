import 'package:flutter/material.dart';
import 'package:provider/provider.dart';
import 'package:t9n_manager_flutter_client/domains/project/models/project.dart';
import 'package:t9n_manager_flutter_client/domains/tenant/models/tenant.dart';
import '../../../generated/l10n.dart';
import '../../../shared/app_state_notifier.dart';
import '../widgets/home.dart';

class HomeScreen extends StatefulWidget {
  const HomeScreen({Key? key}) : super(key: key);

  @override
  State<HomeScreen> createState() => _HomeScreenState();
}

class _HomeScreenState extends State<HomeScreen> {
  @override
  Widget build(BuildContext context) {
    Tenant tenant = context.watch<AppState>().selectedTenant;
    Project project = context.watch<AppState>().selectedProject;
    return Scaffold(
      appBar: AppBar(
        title: Text(S.of(context).title),
      ),
      body: Column(
        children: [
          Padding(
            padding: const EdgeInsets.all(8.0),
            child: Text("Selected tenant/project : ${tenant.name}/${project.name}"),
          ),
          const Home(),
        ],
      ),
    );
  }
}
